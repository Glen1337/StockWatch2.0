using Alpaca.Markets;
using DegenApp.Data;
using DegenApp.Models;
using IEXSharp;
using IEXSharp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEXSharp.Model.CoreData.Options.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfoliosController> _logger;
        private readonly IAlpacaDataClient _alpacaDataClient;
        //private readonly IAlpacaTradingClient _alpacaTradingClient;
        private readonly IEXCloudClient _iexClient;

        public OptionsController(
            ApplicationDbContext context,
            ILogger<PortfoliosController> logger,
            IAlpacaDataClient alpacaDataClient,
            //IAlpacaTradingClient alpacaTradingClient,
            IEXCloudClient iexClient)
        {
            _logger = logger;
            _context = context;
            _iexClient = iexClient;
            //_alpacaTradingClient = alpacaTradingClient;
            _alpacaDataClient = alpacaDataClient;
        }

        // GET: api/OptionsController
        //Get List of Exp. Dates
        [Authorize]
        [HttpGet("Expiry/")]
        public async Task<ActionResult<IAsyncEnumerable<string>>> GetExpiryDates([FromQuery(Name = "symbol")] string symbol)
        {
            try
            {
                _logger.LogInformation("Getting Option expiry dates");

                symbol = symbol.Trim().ToUpper();

                if (!await EquityExistsAsync(symbol))
                {
                    return NotFound("No Equity with this symbol could be found");
                }

                IEXResponse<IEnumerable<string>> expiryDatesResponse = await _iexClient.Options.OptionsAsync(symbol);

                IEnumerable<string> optionExpiryDates = expiryDatesResponse.Data;

                return Ok(optionExpiryDates);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        // GET api/OptionsController/5
        [Authorize]
        [HttpGet("Chain/")]
        public async Task<ActionResult<IAsyncEnumerable<OptionResponse>>> GetChain([FromQuery(Name = "symbol")] string symbol, [FromQuery(Name = "expiration")] string expiration)
        {
            try
            {
                _logger.LogInformation($"Getting ${symbol} Options for expiry: {expiration}");

                if ( String.IsNullOrWhiteSpace(symbol) || !await EquityExistsAsync(symbol))
                {
                    return NotFound("No Equity with this symbol could be found");
                }

                symbol = symbol.Trim().ToUpper();

                IEXResponse<IEnumerable<OptionResponse>> optionsResponse = await _iexClient.Options.OptionsAsync(symbol, expiration);

                IEnumerable<OptionResponse> options = optionsResponse.Data;

                return Ok(options);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
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

    }
}
