using System;
using Xunit;

namespace Technopia.BitcoinConverter.Tests
{
    public class BitCoinConvetetShould
    {
        [Fact]
        public void GetExchangeRate_USD_ReturnsUSDExchangeRate()
        {
            //Arange
            var conveterSvc = new ConvertSvc();

            //Act
            var exchangeRate = conveterSvc.GetExchangeRate("USD");

            //Assert
            var expectedRate = 100;
            Assert.Equal(expectedRate,exchangeRate);
        }
    }
}
