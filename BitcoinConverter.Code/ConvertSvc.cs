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
        public ConvertSvc(){

        }
    
        public async Task<double> GetExchangeRate(string currency) {
            var response = await new HttpClient().GetStringAsync(BITCOIN_CURRRENT_PRICEURL);
            var jsonDocument = JsonDocument.Parse(Encoding.ASCII.GetBytes(response));
            
            if(currency.Equals("USD")){
                var rate = jsonDocument.RootElement.GetProperty("bpi").GetProperty(currency).GetProperty("rate");
                Console.WriteLine(Double.Parse(rate.GetString()));
                return Double.Parse(rate.GetString());
            }
            else if(currency.Equals("GBP")){
                var rate = jsonDocument.RootElement.GetProperty("bpi").GetProperty(currency).GetProperty("rate");
                return Double.Parse(rate.GetString());
            }
            else if(currency.Equals("EUR")){
                var rate = jsonDocument.RootElement.GetProperty("bpi").GetProperty(currency).GetProperty("rate");
                return Double.Parse(rate.GetString());
            }
            return 0;
        }

        public async Task<double> ConvertBitcoins(string currency, double coins)
        {
            var exchangeRate = await GetExchangeRate(currency);

            return exchangeRate*coins;
        }

    }
}
