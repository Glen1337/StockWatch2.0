using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DegenApp.Data;
using DegenApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Alpaca.Markets;
using DegenApp.Scraper;
using System.Collections.Concurrent;
using System.Diagnostics;
using DegenApp.Enums;
using IEXSharp;
using IEXSharp.Model.CoreData.Options.Request;
using IEXSharp.Model.CoreData.Options.Response;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cors;

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfoliosController> _logger;
        private readonly IAlpacaDataClient _alpacaDataClient;
        private readonly IEXCloudClient _iexClient;
        //private readonly IOptionScraper _optionScraper;

        public PortfoliosController(ApplicationDbContext context,
                                    ILogger<PortfoliosController> logger,
                                    IAlpacaDataClient alpacaDataClient,
                                    IEXCloudClient iexClient)
        {
            _logger = logger;
            _context = context;
            _alpacaDataClient = alpacaDataClient;
            _iexClient = iexClient;
            //_optionScraper = optionScraper;
        }

        // GET: api/Portfolios
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Portfolio>>> GetPortfolios()
        {
            _logger.Log(LogLevel.Information, "In Portfolios List - Get");

            string accountID = await GetCurrentUserIdAsync();

            return await _context.Portfolios
                         .Where(port => String.Equals(port.UserId, accountID))
                         .Include(port => port.Holdings)
                         .Include(port => port.Orders)
                         .OrderBy(p => p.PortfolioId)
                         .AsSplitQuery()
                         .ToListAsync();
        }

        // GET: api/Portfolios/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Portfolio>> GetPortfolio(long id)
        {
            _logger.Log(LogLevel.Information, "In Portfolio - Get");

            var timer = new Stopwatch();
            timer.Start();

            string accountID = await GetCurrentUserIdAsync();

            Portfolio portfolio = await _context.Portfolios
            .Include(port => port.Holdings)
            .Include(port => port.Orders)
            .AsSplitQuery()
            .OrderBy(p => p.PortfolioId)
            .SingleOrDefaultAsync(port => long.Equals(port.PortfolioId, id) && String.Equals(port.UserId, accountID));

            if (portfolio == null || portfolio == default(Portfolio))
            {
                return NotFound("The Portfolio you are attempting to access does not exist or cannot be accessed.");
            }

            // For each holding in the portfolio, get the current market price from the API asynchronously
            Portfolio updatedPort = await UpdatePortfolioMarketPricesAsync(portfolio);

            updatedPort.TotalMarketValue = CalculatePortfolioMarketValue(updatedPort);

            updatedPort.GainLoss = updatedPort.TotalMarketValue - CalculatePortfolioCostBasis(updatedPort);

            timer.Stop();
            _logger.LogInformation("Time taken for port. prices update: " + timer.Elapsed.TotalSeconds.ToString());

            return updatedPort;
        }

        // PUT: api/Portfolios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPortfolio(long id, Portfolio portfolio)
        {
            _logger.Log(LogLevel.Information, "In Portfolio - Put");

            if (id != portfolio.PortfolioId || !await PortfolioExistsAsync(id))
            {
                return BadRequest();
            }

            _context.Entry(portfolio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PortfolioExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Portfolios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Portfolio>> PostPortfolio(Portfolio portfolio)
        {
            _logger.Log(LogLevel.Information, "In Portfolio - Post");
            
            string accountID = await GetCurrentUserIdAsync();

            if (await _context.Portfolios.AnyAsync(p => String.Equals(p.UserId, accountID) && String.Equals(p.Title, portfolio.Title)))
            {
                return NotFound("A Portfolio with this name already exists");
            }

            Portfolio newPort = new Portfolio
            {
                TotalMarketValue = 0.0M,
                GainLoss = 0.0M,
                Title = portfolio.Title,
                Type = portfolio.Type,
                CreationDate = DateTime.Now,
                UserId = await GetCurrentUserIdAsync()
            };

            await _context.Portfolios.AddAsync(newPort);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPortfolio", new { id = newPort.PortfolioId }, newPort);
        }

        // DELETE: api/Portfolios/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortfolio(long id)
        {
            _logger.Log(LogLevel.Information, "In Portfolio - Delete");

            string accountID = await GetCurrentUserIdAsync();

            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null || !String.Equals(portfolio.UserId, accountID))
            {
                return NotFound("Could not find portfolio to remove");
            }

            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> PortfolioExistsAsync(long id)
        {
            return await _context.Portfolios.AnyAsync(e => e.PortfolioId == id);
        }

        private async IAsyncEnumerable<Holding> GetAsyncEnumerable(List<Holding> holdings, IAlpacaDataClient adc, IEXCloudClient iexc)
        {
            foreach( Holding holding in holdings)
            {
                //_logger.LogInformation("holding: " + holding.ContractName);
                OptionSide side;
                if (holding.SecurityType == SecurityType.Share)
                {
                    var quote = await adc.GetLastTradeAsync(holding.Symbol.ToUpper());
                    holding.CurrentPrice = quote.Price;
                }
                else
                {
                    if (holding.SecurityType == SecurityType.Put)
                    {
                        side = OptionSide.Put;
                    }

                    else
                    {
                        side = OptionSide.Call;
                    }

                    try
                    {
                        if (holding.ExpirationDate < DateTime.Now)
                        {
                            _logger.LogWarning("\nPossible Expired Option: " + holding.ContractName);
                            holding.CurrentPrice = 0m;
                        }
                        else
                        {
                            var optionResult = await iexc.Options.OptionsAsync(holding.Symbol.Trim().ToUpper(), holding.ExpirationDate.ToString("yyyyMMdd"), side);
                            IEnumerable<OptionResponse> foundOptions = optionResult.Data.Where(o => o.strikePrice == holding.StrikePrice);
                            if (foundOptions.Any())
                            {
                                holding.CurrentPrice = foundOptions.First().closingPrice;
                            }
                            else
                            {
                                _logger.LogWarning("\n\n--------\nUnfound Option: " + holding.ContractName + "\n-----------\n\n");
                                holding.CurrentPrice = 0m;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        _logger.LogError("Option Exception: " + holding.ContractName);
                        holding.CurrentPrice = 0m;
                    }
                }
                yield return holding;
            };
        }

        private async Task<Portfolio> UpdatePortfolioMarketPricesAsync(Portfolio port)
        {
            _logger.LogInformation($"Updating holding values with current market prices for portfolio: {port.Title}");
            //var timer = new Stopwatch();
            //timer.Start();

            var en = GetAsyncEnumerable(port.Holdings.ToList(), _alpacaDataClient, _iexClient);

            await foreach (Holding holding in en)
            {
                //NO-OP
            }

            //timer.Stop();
            //_logger.LogInformation("Time taken: " + timer.Elapsed.TotalSeconds.ToString());
            
            return port;
        }

        private decimal CalculatePortfolioCostBasis(Portfolio port)
        {
            decimal portTotalCostBasis = 0.0m;

            // Get total cost basis for this portfolio
            port.Holdings.ToList<Holding>().ForEach(holding =>
            {
                if (holding.SecurityType == SecurityType.Call || holding.SecurityType == SecurityType.Put)
                {
                    portTotalCostBasis += (Convert.ToDecimal(holding.Quantity) * holding.CostBasis) * 100m;
                }
                else
                {
                    portTotalCostBasis += Convert.ToDecimal(holding.Quantity) * holding.CostBasis;
                }
            });

            return portTotalCostBasis;
        }

        private decimal CalculatePortfolioMarketValue(Portfolio port)
        {
            decimal portMarketValue = 0.0m;

            port.Holdings.ToList<Holding>().ForEach(holding =>
            {
                if (holding.SecurityType == SecurityType.Call || holding.SecurityType == SecurityType.Put)
                {
                    portMarketValue += holding.CurrentPrice * Convert.ToDecimal(holding.Quantity) * 100m;
                }
                else
                {
                    portMarketValue += holding.CurrentPrice * Convert.ToDecimal(holding.Quantity);
                }
            });

            return portMarketValue;
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            String userId = "";

            ClaimsPrincipal currUser = this.User;
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)currUser.Identity;

            Claim userClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            User foundUser = await _context.Users.FindAsync(userClaim.Value);
            if (foundUser == null && userClaim != null)
            {
                var newId = currUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _context.AddAsync<User>(new Models.User { UserId = newId, Balance = 10000m });
                await _context.SaveChangesAsync();
            }

            if (userClaim != null)
            {
                userId = userClaim.Value;
            }

            return userId;
        }

        private object Extract(string os = "", OptionResponse or = null)
        {
            Regex rx = new Regex(@"^(?<ticker>[a-zA-Z]+)(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?<side>[a-zA-Z]{1})(?<price>\d{8})$");

            MatchCollection matches = rx.Matches(or?.id ?? os);

            GroupCollection groups = matches[0].Groups;
            decimal strike = Convert.ToDecimal(groups[6].Value) / 1000m;
            return new
            {
                Ticker = groups[1],
                YearExp = groups[2],
                MonthExp = groups[3],
                DayExp = groups[4],
                Side = groups[5],
                Strike = strike
            };
        }
    }
    
}
