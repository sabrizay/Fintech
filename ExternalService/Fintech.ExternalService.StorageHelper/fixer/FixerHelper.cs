using Fintech.ExternalService.StorageHelper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.ExternalService.StorageHelper.fixer
{
    internal class FixerHelper
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        private readonly string _apiKey;

        public FixerHelper()
        {
            _apiKey = "yAmTJ79keLhZEbamv6Z1k2pIqkQI4S6N";
            _baseUrl = "https://api.apilayer.com";

            _httpClient = new()
            {

            };

            _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
        }


        internal async Task<FixerConvertResponseModel> ConvertCruncy(string key,string amount)
        {
            try
            {
                string url = $"{_baseUrl}/fixer/convert?to=EUR&from={key}&amount={amount}";

                var httpResponse = await _httpClient.GetAsync(url);

                var jsonResult = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FixerConvertResponseModel>(jsonResult);
            }
            catch (Exception ex)
            {

                return default;

            }



            //var result = JsonConvert.DeserializeObject<CloudflareResponseModel>(jsonResult, new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore,
            //});


        }
        internal async Task<FixerSymbolsModel> GetAllCruncy()
        {
            try
            {
                string url = $"{_baseUrl}/fixer/latest";

                var httpResponse = await _httpClient.GetAsync(url);

                var jsonResult = await httpResponse.Content.ReadAsStringAsync();
                return FixerConvertJson(jsonResult);
            }
            catch (Exception ex)
            {

                return default;

            }



            //var result = JsonConvert.DeserializeObject<CloudflareResponseModel>(jsonResult, new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore,
            //});


        }
        public decimal? CustomParse(string incomingValue)
        {
            decimal val;
            if (!decimal.TryParse(incomingValue.Replace(",", "").Replace(".", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                return null;
            return val / 100;
        }
        private FixerSymbolsModel FixerConvertJson(string Json)
        {
            FixerSymbolsModel fixerSymbolsModel = new FixerSymbolsModel();
            var Parse = Json.Split("\"rates\":");
            if (Parse[0].Contains("true"))
                fixerSymbolsModel.success = true;

            string ListJson = Parse[Parse.Length - 1];
            var ListParse = ListJson.Split(",");
            try
            {
                foreach (var item in ListParse)
                {
                    var ItemParse = item.Split(":");
                    string name = ItemParse[0].Replace("}", "").Replace("\"", "").Replace(" ", "").Replace("{", "").Trim();
                    string Value = ItemParse[1].Replace("}", "").Replace("\"", "").Replace(" ", "").Replace("{", "").Trim();
                 

                 
                    fixerSymbolsModel.rates.Add(new rates { Name = name, Value = Value });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
         
            return fixerSymbolsModel;
        }
    }
}
