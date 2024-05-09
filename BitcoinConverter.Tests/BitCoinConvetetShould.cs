using System;
using System.Threading.Tasks;
using Xunit;

namespace Technopia.BitcoinConverter.Tests
{
    public class BitCoinConveterShould
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
        [Fact]
        public void GetExchangeRate_EUR_ReturnsUSDExchangeRate()
        {
            //Arange
            var conveterSvc = new ConvertSvc();

            //Act
            var exchangeRate = conveterSvc.GetExchangeRate("EUR");

            //Assert
            var expectedRate = 150;
            Assert.Equal(expectedRate,exchangeRate);
        }

        [Fact]
        public void GetExchangeRate_GBP_ReturnsGBPExchangeRate()
        {
            //Arange
            var conveterSvc = new ConvertSvc();

            //Act
            var exchangeRate = conveterSvc.GetExchangeRate("GBP");

            //Assert
            var expectedRate = 200;
            Assert.Equal(expectedRate,exchangeRate);
        }

        [Theory]
        [InlineData("USD",1,100)]
        [InlineData("USD",2,200)]
        [InlineData("EUR",1,150)]
        [InlineData("EUR",2,300)]
        [InlineData("GBP",1,200)]
        [InlineData("GBP",2,400)]
        public async Task ConvertBitcoins_BitCoinToCurrency_ReturnsCurrency(string currency, int coins, int convertedCurrency)
        {
            // Given
            var conveterSvc = new ConvertSvc();
            // When
            var converted = await conveterSvc.ConvertBitcoins(currency,coins);
            // Then
            Assert.Equal(convertedCurrency,converted);
        }
    }
}
