using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Technopia.BitcoinConverter
{
    public class ConvertSvc
    {
        readonly string BITCOIN_CURRRENT_PRICEURL = "https://api.coindesk.com/v1/bpi/currentprice.json";
        private HttpClient httpClient;
        public ConvertSvc(){
            this.httpClient = new HttpClient();
        }

        public ConvertSvc(HttpClient client)
        {
            this.httpClient = client;
        }

        public enum Currency
        {
            USD,
            GBP,
            EUR,
        }
        public async Task<double> GetExchangeRate(Currency currency) {

            double rate = 0;

            try
            {
                var response = await this.httpClient.GetStringAsync(BITCOIN_CURRRENT_PRICEURL);
                var jsonDocument = JsonDocument.Parse(Encoding.ASCII.GetBytes(response));

                var rateStr = jsonDocument.RootElement.GetProperty("bpi").GetProperty(currency.ToString()).GetProperty("rate");
                
                rate = Double.Parse(rateStr.GetString());
            }
            catch (Exception)
            {

                return -1;
            }
            
            return Math.Round(rate,4);
        }

        public async Task<double> ConvertBitcoins(Currency currency, double coins)
        {
            double dollars = 0;
            var exchangeRate = await GetExchangeRate(currency);

            if (coins <0)
            {
                throw new ArgumentException("Number of coins should be zero");
            }

            if (exchangeRate > 0)
            {
                dollars = exchangeRate * coins;
            }
            else
            {
                return -1;
            }

            return dollars;
        }

    }
}
