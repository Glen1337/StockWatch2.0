using Alpaca.Markets;
using DegenApp.Data;
using IEXSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DegenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfoliosController> _logger;
        private readonly IAlpacaDataClient _alpacaDataClient;
        //private readonly IAlpacaTradingClient _alpacaTradingClient;
        private readonly IEXCloudClient _iexClient;

        public ResearchController(
            //ApplicationDbContext context,
            ILogger<PortfoliosController> logger,
            IAlpacaDataClient alpacaDataClient,
            //IAlpacaTradingClient alpacaTradingClient,
            IEXCloudClient iexClient)
        {
            _logger = logger;
            //_context = context;
            _alpacaDataClient = alpacaDataClient;
            //_alpacaTradingClient = alpacaTradingClient;
            _iexClient = iexClient;
        }

        // GET: api/Research/Chart/symbol
        //[Authorize]
        [Route("Chart")]
        [HttpGet()]
        public async Task<ActionResult<IReadOnlyCollection<IAgg>>> GetResearchData([FromQuery(Name ="symbol")] string symbol)
        {
            string ticker = symbol.Trim().ToUpper();
            
            if(! await EquityExistsAsync(ticker))
            {
                return NotFound("No Equity with this symbol could be found");
            }

            _logger.LogInformation(HttpContext.Request.ToString()); //QueryString.ToString());
            var bars = await _alpacaDataClient.GetBarSetAsync(new BarSetRequest(ticker, TimeFrame.Day) { Limit = 45 });
            //string stockTicker = bars.ElementAt(0).Key;
            IReadOnlyList<IAgg> actualBars = bars.ElementAt(0).Value;

            if (actualBars.Count > 1) { _logger.LogInformation($"Retrieved bars for {ticker}"); }
            if (actualBars.Count < 1) { _logger.LogInformation($"Failed to retrieve bars for {ticker}"); }

            return Ok(actualBars);
        }

        // GET: api/Research/Market/symbol
        [Route("Market")]
        [HttpGet()]
        public async Task<ActionResult> GetMarketData()
        {
            try
            {
                var fedFundsRateTask = _iexClient.EconomicData.DataPointAsync(IEXSharp.Model.CoreData.EconomicData.Request.EconomicDataSymbol.FEDFUNDS);
                //Now requires additional entitlements
                //var earningsReleasesTask = _iexClient.MarketInfoService.EarningsTodayAsync();
                var sectorPerformanceTask = _iexClient.MarketInfoService.SectorPerformanceAsync();
                // currently down
                //var ipoTask = _iexClient.MarketInfoService.IPOCalendarAsync;
                //var cryptoTask = _iexClient.Crypto.PriceAsync("BTCUSD");
                var crashTask = _iexClient.EconomicData.DataPointAsync(IEXSharp.Model.CoreData.EconomicData.Request.EconomicDataSymbol.RECPROUSM156N);
                var institutionTask = _iexClient.EconomicData.DataPointAsync(IEXSharp.Model.CoreData.EconomicData.Request.EconomicDataSymbol.WIMFSL);

                //4 indices
                var sp500Task = _iexClient.StockPrices.QuoteAsync("SPY");
                var russellTask = _iexClient.StockPrices.QuoteAsync("IWM");
                var dowTask = _iexClient.StockPrices.QuoteAsync("DIA");
                var nasdaqTask = _iexClient.StockPrices.QuoteAsync("QQQ");

                //Task.WaitAll(sectorPerformanceTask, fedFundsRateTask, earningsReleasesTask, crashTask);

                var sectorPerformance = await sectorPerformanceTask;
                var fedFundsRate = await fedFundsRateTask;
                //var crypto = await cryptoTask;
                var crash = await crashTask;
                var institution = await institutionTask;
                var nasdaq = await nasdaqTask;
                var dow = await dowTask;
                var russell = await russellTask;
                var sp500 = await sp500Task;
                //var earnings = await earningsReleasesTask;

                string jsonData = JsonSerializer.Serialize(new
                {
                    federalFundsRate = fedFundsRate.Data,
                    sectorPerformances = sectorPerformance.Data,
                    recessionProbability = crash.Data,
                    //earningsRelease = earnings.Data,
                    //crypto = crypto.Data,
                    instFunds = institution.Data,
                    sp500 = sp500.Data,
                    russell = russell.Data,
                    dow = dow.Data,
                    nasdaq = nasdaq.Data
                });
                return Ok(jsonData);
            }
            catch (JsonException je)
            {
                _logger.LogError(je.Message);
                return BadRequest(je.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        // /api/Research/CompanyStats
        [Route("CompanyStats")]
        [HttpGet()]
        public async Task<ActionResult> GetCompanyData([FromQuery(Name = "symbol")] string symbol)
        {

            if (String.IsNullOrWhiteSpace(symbol) || !await EquityExistsAsync(symbol))
            {
                return NotFound("No Equity with this symbol could be found");
            }

            string ticker = symbol.Trim().ToUpper();

            try
            {
                var upcomingEventsTask = _iexClient.MarketInfoService.UpcomingEventsAsync(ticker);
                var advancedStatsTask = _iexClient.StockResearch.AdvancedStatsAsync(ticker);
                var logoTask = _iexClient.StockProfiles.LogoAsync(ticker);
                var institutionOwnershipTask = _iexClient.StockResearch.InstitutionalOwnerShipAsync(ticker);
                //var newsTask = _iexClient.News.NewsAsync(ticker);
                //var fundOwnershipTask = _iexClient.StockResearch.FundOwnershipAsync(ticker);
                //var companyTask = _iexClient.StockProfiles.CompanyAsync(ticker);

                //Task.WhenAll(advancedStatsTask, logoTask);

                //_logger.LogInformation(HttpContext.Request.ToString());
                //var fundOwnership = await fundOwnershipTask;
                //var company = await companyTask;
                //var news = await newsTask;
                var upcomingEvents = await upcomingEventsTask;
                var advancedStats = await advancedStatsTask;
                var logo = await logoTask;
                var institutionOwnership = await institutionOwnershipTask;

                string jsonData = JsonSerializer.Serialize(new
                {
                    logo = logo.Data,
                    advStats = advancedStats.Data,
                    upcomingEvents = upcomingEvents.Data,
                    instOwnership = institutionOwnership.Data
                    //news = news.Data,
                    //fundOwnership = fundOwnership.Data,
                    //company = company.Data
                });
                return Ok(jsonData);
            }
            catch(JsonException je)
            {
                _logger.LogError(je.Message);
                return BadRequest(je.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
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
