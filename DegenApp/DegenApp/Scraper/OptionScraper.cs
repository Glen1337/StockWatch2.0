//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using System.Net;
//using System.Text;
//using System.IO;
//using System.Collections.Generic;
//using HtmlAgilityPack;
//using Microsoft.Extensions.Logging;
//using DegenApp.Models;
//using System;
//using System.Collections.Concurrent;
//using System.Linq;

//namespace DegenApp.Scraper
//{
//    public class OptionScraper : IOptionScraper
//    {
//        private readonly ILogger<OptionScraper> _logger;

//        public OptionScraper(ILogger<OptionScraper> logger)
//        {
//            _logger = logger;
//        }

//        public async Task<string> GetOptionsDataAsync(string symbol)
//        {
//            string _symbol = symbol.ToUpper().Trim();
//            HttpClient httpClient = new HttpClient();
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
//            httpClient.DefaultRequestHeaders.Accept.Clear();
//            var response = await httpClient.GetStringAsync($"https://finance.yahoo.com/quote/{_symbol}/options?p={_symbol}");
//            List<Option> result = await GetOptionsFromHtmlAsync(response);

//            _logger.LogInformation(result.ToString()); 
//            return "";
//        }

//        public decimal FindSpecificOptionPrice(string input)
//        {
//            return 0.0m;
//        }

//        private async Task<List<Option>> GetOptionsFromHtmlAsync(string inputHtml)
//        {
//            ConcurrentBag<Option> optionBag = new ConcurrentBag<Option>();
            
//            HtmlDocument htmlDoc = new HtmlDocument();
//            htmlDoc.LoadHtml(inputHtml);
            
//            // Add calls
//            HtmlNode calls = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]/div[1]/div[1]/section[1]/section[1]/div[2]/div[1]/table[1]/tbody[1]");

//            Parallel.ForEach(calls.ChildNodes, call =>
//            {
//                Option optionToAdd = new Option
//                {
//                    ContractName = call.ChildNodes[0].InnerText,
//                    Strike = GetDecimalFromText(call.ChildNodes[2].InnerText),
//                    LastPrice = GetDecimalFromText(call.ChildNodes[3].InnerText),
//                    Bid = Convert.ToDecimal(call.ChildNodes[4].GetDirectInnerText()),
//                    Ask = GetDecimalFromText(call.ChildNodes[5].InnerText),
//                    Change = GetDecimalFromText(call.ChildNodes[6].InnerText),
//                    PercentageChange = GetDecimalFromTextWithPercentage(call.ChildNodes[7].InnerText),
//                    Volume = GetLongFromText(call.ChildNodes[8].InnerText),
//                    OpenInterest = GetLongFromText(call.ChildNodes[9].InnerText),
//                    ImpliedVolatility = GetDecimalFromTextWithPercentage(call.ChildNodes[10].InnerText),
//                    OptionType = Enums.OptionType.Call
//                };
//                optionBag.Add(optionToAdd);
//            });

//            // Add puts
//            HtmlNode puts  = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]/div[1]/div[1]/section[1]/section[2]/div[2]/div[1]/table[1]/tbody[1]");
            
//            Parallel.ForEach(puts.ChildNodes, put =>
//            {
//                Option optionToAdd = new Option
//                {
//                    ContractName = put.ChildNodes[0].InnerText,
//                    Strike = GetDecimalFromText(put.ChildNodes[2].InnerText),
//                    LastPrice = GetDecimalFromText(put.ChildNodes[3].InnerText),
//                    Bid = GetDecimalFromText(put.ChildNodes[4].InnerText),
//                    Ask = GetDecimalFromText(put.ChildNodes[5].InnerText),
//                    Change = GetDecimalFromText(put.ChildNodes[6].InnerText),
//                    PercentageChange = GetDecimalFromTextWithPercentage(put.ChildNodes[7].InnerText),
//                    Volume = GetLongFromText(put.ChildNodes[8].InnerText),
//                    OpenInterest = GetLongFromText(put.ChildNodes[9].InnerText),
//                    ImpliedVolatility = GetDecimalFromTextWithPercentage(put.ChildNodes[10].InnerText),
//                    OptionType = Enums.OptionType.Put
//                };
//                optionBag.Add(optionToAdd);
//            });

//            return optionBag.ToList<Option>();
//        }

//        private Decimal GetDecimalFromText(string input)
//        {
//            string fixedString = new String(input.Trim());

//            if (String.Equals(fixedString, "-"))
//            {
//                return 0.0m;
//            }

//            if (!(fixedString.IndexOf("+") == -1))
//            {
//                fixedString = input.Remove(input.IndexOf("+"), 1);
//            }

//            if (!(fixedString.IndexOf("-") == -1))
//            {
//                fixedString = input.Remove(input.IndexOf("-"), 1);
//            }

//            if (!(fixedString.IndexOf("%") == -1))
//            {
//                fixedString = input.Remove(input.IndexOf("%"), 1);
//            }

//            return Convert.ToDecimal(fixedString);
//        }

//        private Int64 GetLongFromText(string input)
//        {
//            return String.Equals(input, "-") ? 0L : Convert.ToInt64(input.Trim());
//        }

//        private Decimal GetDecimalFromTextWithPercentage(string inputText)
//        {
//            string input = inputText.Trim();
//            return String.Equals(input, "-") ? 0.0m : Convert.ToDecimal(input.Remove(input.IndexOf("%"), 1));
//        }


//    }
//}
