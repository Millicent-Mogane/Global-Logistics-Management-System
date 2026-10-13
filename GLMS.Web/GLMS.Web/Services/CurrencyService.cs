using System.Net.Http;
using System.Text.Json;

namespace GLMS.Web.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            var response = await _httpClient.GetStringAsync(
                "https://open.er-api.com/v6/latest/USD");

            using JsonDocument document = JsonDocument.Parse(response);

            decimal rate =
                document.RootElement
                .GetProperty("rates")
                .GetProperty("ZAR")
                .GetDecimal();

            return rate;
        }
    }
}
