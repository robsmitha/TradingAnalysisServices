using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stocks.Application.Common.Interfaces;
using Stocks.Domain.Data;
using Stocks.Domain.Models;
using Stocks.Infrastructure.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Infrastructure.Services
{
    public class IEXCloudService : IIEXCloudService
    {
        private readonly HttpClient _client;
        private readonly AppSettings _appSettings;

        public IEXCloudService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _client = httpClient;
            _appSettings = appSettings.Value;
        }

        private string RequestUri(string endpoint, Dictionary<string, string> @params)
        {
            var requestUri = new StringBuilder();
            requestUri.Append(_appSettings.BaseUrl);
            requestUri.Append($"/{_appSettings.Version}");
            requestUri.Append($"/{endpoint}");
            requestUri.Append($"?token={_appSettings.Token}");
            @params?.Keys.ToList().ForEach(k => requestUri.Append($"&{k}={@params[k]}"));
            return requestUri.ToString();
        }

        private async Task<T> SendAsync<T>(string endpoint, Dictionary<string, string> @params = null)
        {
            try
            {
                var uri = RequestUri(endpoint, @params);
                var response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(result);
                }

                return default;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<List<StockPrice>> GetHistoricalPrices(string symbol, string range = "5d")
        {
            var stocks = await SendAsync<List<StockPrice>>(
                    endpoint: $"stock/{symbol}/chart/{range}",
                    @params: new Dictionary<string, string> { { "includeToday", "false" } });
            return stocks;
        }

        public async Task<CandleStickChart> GetCandleStickChart(string symbol, string range)
        {
            var prices = await GetHistoricalPrices(symbol, range);
            return new CandleStickChart(symbol, range, prices);
        }
    }
}
