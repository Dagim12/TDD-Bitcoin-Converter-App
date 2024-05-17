using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
namespace Technopia.BitcoinConverter.Tests
{
    public class BitCoinConveterShould
    {
        private const string MOCK_RESPONSE_JSON = "{"time":{"updated":"May 17, 2024 15:58:15 UTC","updatedISO":"2024-05-17T15:58:15+00:00","updateduk":"May 17, 2024 at 16:58 BST"},"disclaimer":"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org","chartName":"Bitcoin","bpi":{"USD":{"code":"USD","symbol":"&#36;","rate":"67,202.397","description":"United States Dollar","rate_float":67202.3972},"GBP":{"code":"GBP","symbol":"&pound;","rate":"52,924.24","description":"British Pound Sterling","rate_float":52924.2399},"EUR":{"code":"EUR","symbol":"&euro;","rate":"61,787.295","description":"Euro","rate_float":61787.2952}}}";
        private ConverterSVC mockConverterSVC;
        public BitCoinConverterShoul(){
            mockConverterSVC= GetMockBitcoinConverterService();
        }

        private ConverterSVC GetMockBitcoinConverterService(){
            var handlerMock = new Mock<HttpMessageHandler>();

            var response = new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK;
                Content = new StringContent(MOCK_RESPONSE_JSON);
            }

            handlerMock
                .protected();
                .setup<taks<HttpResponseMessage>(
                    "SendAssync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());
               .ReturnsAsync(response)

               var httpClient = 
        }
        [Fact]
        public async Task GetExchangeRate_USD_ReturnsUSDExchangeRate()
        {
            //Arange
            var conveterSvc = new ConvertSvc();

            //Act
            double exchangeRate = await conveterSvc.GetExchangeRate("USD");

            //Assert
            double expectedRate = 100;
            Assert.Equal(expectedRate,exchangeRate);
        }
        [Fact]
        public async Task GetExchangeRate_EUR_ReturnsUSDExchangeRate()
        {
            //Arange
            var conveterSvc = new ConvertSvc();

            //Act
            var exchangeRate = await conveterSvc.GetExchangeRate("EUR");

            //Assert
            var expectedRate = 150;
            Assert.Equal(expectedRate,exchangeRate);
        }

        [Fact]
        public async Task GetExchangeRate_GBP_ReturnsGBPExchangeRate()
        {
            //Arange
            var conveterSvc = new ConvertSvc();

            //Act
            var exchangeRate = await conveterSvc.GetExchangeRate("GBP");

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
