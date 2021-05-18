using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DegenApp.Data;
using DegenApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Alpaca.Markets;
using IEXSharp;
using IEXSharp.Model;
using IEXSharp.Model.Shared.Response;

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WatchItemsController> _logger;
        private readonly IEXCloudClient _iexClient;
        private readonly IAlpacaDataClient _alpacaDataClient;


        public WatchItemsController(ApplicationDbContext context, ILogger<WatchItemsController> logger, IEXCloudClient iexClient, IAlpacaDataClient alpacaDataClient)
        {
            _context = context;
            _logger = logger;
            _iexClient = iexClient;
            _alpacaDataClient = alpacaDataClient;
        }

        // GET: api/WatchItems
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<WatchItem>>> GetWatchItems()
        {
            _logger.Log(LogLevel.Information, "In WatchItems - Get");

            string accountID = await GetCurrentUserIdAsync();

            var items = _context.WatchItems
                        .Where(watchItem => String.Equals(watchItem.UserId, accountID)).AsAsyncEnumerable();

            await foreach (var watchItem in items)
            {
                IEXResponse<Quote> quote = await _iexClient.StockPrices.QuoteAsync(watchItem.Symbol);
                watchItem.PercentChange = quote.Data.changePercent * 100m ?? 0.0m;
                watchItem.PriceChange = quote.Data.change ?? 0.0m;
            }

            return Ok(items);

                //return await _context.WatchItems
                //        .Where(watchItem => String.Equals(watchItem.UserId, accountID))
                //        .ToListAsync();
        }

        // POST: api/WatchItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<WatchItem>> PostWatchItem(WatchItem inputWatchItem)
        {
            _logger.Log(LogLevel.Information, "In WatchItem - Post");

            string accountID = await GetCurrentUserIdAsync();

            if (WatchItemExists(inputWatchItem.Symbol, accountID))
            {
                return BadRequest("Watch Item already exists");
            }

            if (!await EquityExistsAsync(inputWatchItem.Symbol))
            {
                return NotFound("No Equity with this symbol could be found");
            }

            WatchItem watchItem = new WatchItem
            {
                Outlook = inputWatchItem.Outlook,
                Symbol = inputWatchItem.Symbol.Trim().ToUpper(),
                UserId = accountID
            };

            _context.WatchItems.Add(watchItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostWatchItem", new { id = watchItem.WatchItemId }, watchItem);
        }

        // DELETE: api/WatchItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchItem(long id)
        {
            var watchItem = await _context.WatchItems.FindAsync(id);
            if (watchItem == null)
            {
                return NotFound();
            }

            _context.WatchItems.Remove(watchItem);
            await _context.SaveChangesAsync();

            return NoContent();
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

        private bool WatchItemExists(string inputSymbol, string accountID)
        {
            string symbol = inputSymbol.Trim().ToUpper();
            return _context.WatchItems.Any(e => e.Symbol == symbol && String.Equals(e.UserId, accountID));
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
                await _context.AddAsync<User>(new Models.User { UserId = newId, Balance = 10000 });
                await _context.SaveChangesAsync();
            }

            if (userClaim != null)
            {
                userId = userClaim.Value;
            }

            return userId;
        }

        #region unused
        // GET: api/WatchItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<WatchItem>> GetWatchItem(long id)
        //{
        //    var watchItem = await _context.WatchItems.FindAsync(id);

        //    if (watchItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return watchItem;
        //}

        // PUT: api/WatchItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutWatchItem(long id, WatchItem watchItem)
        //{
        //    if (id != watchItem.WatchItemId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(watchItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!WatchItemExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        #endregion
    }
}
