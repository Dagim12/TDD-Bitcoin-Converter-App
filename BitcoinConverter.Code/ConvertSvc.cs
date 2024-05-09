using System;
using System.Threading.Tasks;

namespace Technopia.BitcoinConverter
{
    public class ConvertSvc
    {
        public ConvertSvc(){

        }

        public int GetExchangeRate(string currency) {
            if(currency.Equals("USD")){
                return 100;
            }
            else if(currency.Equals("GBP")){
                return 200;
            }
            else if(currency.Equals("EUR")){
                return 150;
            }
            return 0;
        }

        public async Task<double> ConvertBitcoins(string currency, double coins)
        {
            var exchangeRate = GetExchangeRate(currency);

            return exchangeRate*coins;
        }

    }
}
