using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DegenApp.Data;
using DegenApp.Models;
using Alpaca.Markets;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using DegenApp.Enums;
using IEXSharp;
using IEXSharp.Model.CoreData.Options.Request;
using IEXSharp.Model.CoreData.Options.Response;
using System.Text.RegularExpressions;

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfoliosController> _logger;
        private readonly IAlpacaDataClient _alpacaDataClient;
        private readonly IAlpacaTradingClient _alpacaTradingClient;
        private readonly IEXCloudClient _iexClient;

        public HoldingsController(ApplicationDbContext context,
            ILogger<PortfoliosController> logger,
            IAlpacaDataClient alpacaDataClient,
            IAlpacaTradingClient alpacaTradingClient,
            IEXCloudClient iexClient)
        {
            _logger = logger;
            _context = context;
            _alpacaTradingClient = alpacaTradingClient;
            _alpacaDataClient = alpacaDataClient;
            _iexClient = iexClient;
        }

        // GET: api/Holdings
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Holding>>> GetHoldings()
        //{
        //    // Fake user id 
        //    string accountID = "1";
        //    return await _context.Holdings.Where(holding => String.Equals(holding.Portfolio.UserId, accountID)).ToListAsync();
        //}

        // GET: api/Holdings/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Holding>> GetHolding(long id)
        //{
        //    // Fake user id 
        //    string accountID = "1";

        //    var holding = await _context.Holdings.FindAsync(id);

        //    if (holding == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!String.Equals(holding.Portfolio.UserId, accountID))
        //    {
        //        return NotFound("Holding is not owned by current user");
        //    }

        //    return holding;
        //}

        // PUT: api/Holdings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHolding(long id, Holding holding)
        {
            string accountID = await GetCurrentUserIdAsync();

            User user = await _context.Users.FindAsync(accountID);

            if (id != holding.HoldingId)
            {
                return BadRequest("Holding ID mismatch");
            }

            var existingHolding = await _context.Holdings.AsNoTracking<Holding>().FirstOrDefaultAsync(h => h.HoldingId == id);

            if (!_context.Portfolios.Any(p => p.PortfolioId == existingHolding.PortfolioId && p.UserId == accountID))
            {
                return Unauthorized();
            }

            holding.TransactionDate = DateTime.Now;

            holding.CurrentPrice = await GetMarketPriceAsync(holding.Symbol, holding.OrderType==Enums.OrderType.Buy);

            decimal cost = holding.CurrentPrice * Convert.ToDecimal(holding.Quantity);

            if (cost > user.Balance)
            {
                return NotFound("Not enough cash in account to complete order");
            }

            user.Balance -= cost;

            // Create order for order table
            Order order = new()
            {
                Symbol = holding.Symbol,
                PortfolioId = holding.PortfolioId,
                Price = holding.CurrentPrice,
                Quantity = holding.Quantity,
                UserId = accountID,
                TransactionDate = holding.TransactionDate,
                OrderType = holding.OrderType,
                SecurityType = holding.SecurityType
            };

            holding.CostBasis = ((existingHolding.CostBasis * Convert.ToDecimal(existingHolding.Quantity))
                                 +(holding.CurrentPrice * Convert.ToDecimal(holding.Quantity)))
                                /(Convert.ToDecimal(existingHolding.Quantity) + Convert.ToDecimal(holding.Quantity));
            holding.Quantity += existingHolding.Quantity;

            try
            {
                await _context.Orders.AddAsync(order);

                _context.Entry(holding).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoldingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction("PutHolding", new { id = holding.HoldingId }, holding);
        }

        //[Authorize]
        //[Route("Options")]
        //[HttpPost]
        ////GET: api/Holdings/Options
        //public async Task<ActionResult<Holding>> PostOptionHolding(Holding holding)
        //{
        //    string accountID = await GetCurrentUserIdAsync();

        //    User user = await _context.Users.FindAsync(accountID);

        //    holding.TransactionDate = DateTime.Now;

        //    decimal cost = holding.CurrentPrice;

        //    return CreatedAtAction("PostOptionHolding", new { id = holding.HoldingId }, holding);
        //}

        // POST: api/Holdings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Holding>> PostHolding(Holding holding)
        {
            decimal cost = 0.0m;

            string accountID = await GetCurrentUserIdAsync();

            User user = await _context.Users.FindAsync(accountID);

            if (String.IsNullOrWhiteSpace(holding.Symbol) || !await EquityExistsAsync(holding.Symbol))
            {
                return NotFound("No Equity with this symbol could be found");
            }

            holding.Symbol = holding.Symbol.ToUpper().Trim();

            var marketClock = await _alpacaTradingClient.GetClockAsync();// GetClock Async();
            
            if (!marketClock.IsOpen)
            {
                //return NotFound("Unable to place order because the Market is closed.");
                //return NoContent();
            }
            // Set DateTime
            holding.TransactionDate = DateTime.Now;

            // Update CurrentPrice and CostBasis
            if (holding.SecurityType == SecurityType.Share)
            {
                holding.CurrentPrice = await GetMarketPriceAsync(holding.Symbol, true);
                cost = holding.CurrentPrice * Convert.ToDecimal(holding.Quantity);
            }

            if (holding.SecurityType == SecurityType.Call || holding.SecurityType == SecurityType.Put)
            {
                holding.CurrentPrice = await GetOptionPriceAsync(holding, true);
                cost = (holding.CurrentPrice * 100m) * Convert.ToDecimal(holding.Quantity);
            }

            holding.CostBasis = holding.CurrentPrice;

            if (cost > user.Balance)
            {
                return NotFound("Not enough cash in account to complete order");
            }
            user.Balance -= cost;

            // Mark as Open
            holding.IsOpen = true;

            // Create order for order table
            Order order = new()
            {
                Symbol = holding.Symbol,
                PortfolioId = holding.PortfolioId,
                Price = holding.CurrentPrice,
                Quantity = holding.Quantity,
                UserId = accountID,
                TransactionDate = DateTime.Now,
                OrderType = holding.OrderType,
                SecurityType = holding.SecurityType
            };

            //Add to orders table and holdings table as 1 transaction
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.Holdings.AddAsync(holding);
                
                await _context.Orders.AddAsync(order);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("Error creating Order: " + e.Message);
                return BadRequest(e.Message);
            }

            return CreatedAtAction("PostHolding", new { id = holding.HoldingId }, holding);
        } 

        // DELETE: api/Holdings/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHolding(long id)
        {
            decimal cost = 0.0m;

            string accountID = await GetCurrentUserIdAsync();

            User user = await _context.Users.FindAsync(accountID);

            var clock = await _alpacaTradingClient.GetClockAsync();

            if (!clock.IsOpen)
            {
                //return NotFound("Unable to place order because the Market is closed.");
                //return NoContent();
            }
            var holding = await _context.Holdings.FindAsync(id);

            if (holding == null || !await _context.Portfolios.AnyAsync(p => p.PortfolioId == holding.PortfolioId && p.UserId == accountID))
            {
                return NotFound("Could not find holding to sell");
            }

            if(holding.SecurityType == SecurityType.Call || holding.SecurityType == SecurityType.Put)
            {
                holding.CurrentPrice = await GetOptionPriceAsync(holding, false);
                cost = (holding.CurrentPrice * 100m) * Convert.ToDecimal(holding.Quantity);
            }

            if(holding.SecurityType == SecurityType.Share)
            {
                holding.CurrentPrice = await GetMarketPriceAsync(holding.Symbol, false);
                cost = holding.CurrentPrice * Convert.ToDecimal(holding.Quantity);
            }

            // Create order for order table
            Order order = new()
            {
                Symbol = holding.Symbol,
                PortfolioId = holding.PortfolioId,
                Price = holding.CurrentPrice,
                Quantity = holding.Quantity,
                UserId = accountID,
                TransactionDate = DateTime.Now,
                OrderType = Enums.OrderType.Sell,
                SecurityType = holding.SecurityType
            };

            user.Balance += cost;
            
            //Add to orders table and holdings table as 1 transaction
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Holdings.Remove(holding);

                await _context.Orders.AddAsync(order);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error creating Order: " + e.Message);
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        private bool HoldingExists(long id)
        {
            return _context.Holdings.Any(e => e.HoldingId == id);
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
                userId = currUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            return userId;
        }

        /// <summary>
        /// Get current market value of an equity 
        /// </summary>
        /// <param name="inputSymbol"></param>
        /// <returns></returns>
        private async Task<decimal> GetMarketPriceAsync(string inputSymbol, bool buying)
        {
            string symbol = inputSymbol.ToUpper().Trim();

            var marketClock = await _alpacaTradingClient.GetClockAsync();// GetClock Async();

            if (!marketClock.IsOpen)
            {
                return (await _alpacaDataClient.GetLastTradeAsync(symbol)).Price;
                //return NotFound("Unable to place order because the Market is closed.");
                //return NoContent();
            }
            else
            {
                ILastQuote currentQuote = await _alpacaDataClient.GetLastQuoteAsync(symbol);
                if (buying)
                {
                    return currentQuote.AskPrice;
                }
                else
                {
                    return currentQuote.BidPrice;
                }
            }
        }

        private async Task<decimal> GetOptionPriceAsync(Holding holding, bool buying)
        {
            //holding.CurrentPrice = await GetOptionPriceAsync(holding.Symbol, holding.ExpirationDate.ToString("yyyyMMdd"), holding.SecurityType, holding.StrikePrice, true);

            if(holding.ExpirationDate < DateTime.Now)
            {
                _logger.LogWarning("\nExpired Option: " + holding.ContractName);
                return 0m;
            }

            OptionSide side;

            if (holding.SecurityType == SecurityType.Put)
            {
                side = OptionSide.Put;
            }
            else
            {
                side = OptionSide.Call;  
            }

            var optionResult = await _iexClient.Options.OptionsAsync(holding.Symbol.ToUpper().Trim(), holding.ExpirationDate.ToString("yyyyMMdd"), side);

            IEnumerable<OptionResponse> foundOptions = optionResult.Data.Where(o => o.strikePrice == holding.StrikePrice);

            if (foundOptions.Any())
            {
                if (buying)
                {
                    return foundOptions.First().ask;
                }
                else
                {
                    return foundOptions.First().bid;
                }
            }
            else
            {
                _logger.LogWarning("\n\n--------\nUnfound Option: " + holding.ContractName + "within list: \n" + optionResult.Data + "\n-----------\n\n");
                return 0m;
            }
            //return optionResult.Data.First(o => o.strikePrice == strike).ask;
        }

        private async Task<bool> EquityExistsAsync(string inputSymbol)
        {
            try
            {
                string symbol = inputSymbol.Trim().ToUpper();
                ILastTrade lastTrade = await _alpacaDataClient.GetLastTradeAsync(symbol);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private object Extract(string os = "", OptionResponse or = null)
        {
            Regex rx = new Regex(@"^(?<ticker>[a-zA-Z]+)(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?<side>[a-zA-Z]{1})(?<price>\d{8})$");

            MatchCollection matches = rx.Matches(or.id ?? os);

            GroupCollection groups = matches[0].Groups;
            decimal strike = Convert.ToDecimal(groups[6].Value)/1000m;
            var idk = new
            {
                ticker = groups[1],
                yearExp = groups[2],
                monthExp = groups[3],
                dayExp = groups[4],
                side = groups[5],
                strike = strike
            };
            _logger.LogInformation(idk.ToString());
            return idk;
        }
    }
}
