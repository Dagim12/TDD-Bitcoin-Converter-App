using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Technopia.BitcoinConverter.Tests
{
    public class BitCoinConveterShould
    {
        private const string MOCK_RESPONSE_JSON = @"{""time"":{""updated"":""May 17, 2024 17:42:12 UTC"",""updatedISO"":""2024-05-17T17:42:12+00:00"",""updateduk"":""May 17, 2024 at 18:42 BST""},""disclaimer"":""This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org"",""chartName"":""Bitcoin"",""bpi"":{""USD"":{""code"":""USD"",""symbol"":""&#36;"",""rate"":""66,812.726"",""description"":""United States Dollar"",""rate_float"":66812.7259},""GBP"":{""code"":""GBP"",""symbol"":""&pound;"",""rate"":""52,605.267"",""description"":""British Pound Sterling"",""rate_float"":52605.267},""EUR"":{""code"":""EUR"",""symbol"":""&euro;"",""rate"":""61,472.719"",""description"":""Euro"",""rate_float"":61472.7188}}}";
        private ConvertSvc mockConverterSVC;
        public BitCoinConveterShould() {
            mockConverterSVC= GetMockBitcoinConverterService();
        }

        private ConvertSvc GetMockBitcoinConverterService(){
            var handlerMock = new Mock<HttpMessageHandler>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(MOCK_RESPONSE_JSON),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);


            var httpClient = new HttpClient(handlerMock.Object);
            var converter = new ConvertSvc(httpClient);

            return converter;
        }
        [Fact]
        public async Task GetExchangeRate_USD_ReturnsUSDExchangeRate()
        {
            //Act
            double exchangeRate = await mockConverterSVC.GetExchangeRate(ConvertSvc.Currency.USD);

            //Assert
            double expectedRate = 66812.726;
            Assert.Equal(expectedRate,exchangeRate);
        }
        [Fact]
        public async Task GetExchangeRate_EUR_ReturnsUSDExchangeRate()
        {
            //Act
            var exchangeRate = await mockConverterSVC.GetExchangeRate(ConvertSvc.Currency.EUR);

            //Assert
            var expectedRate = 61472.719;
            Assert.Equal(expectedRate,exchangeRate);
        }

        [Fact]
        public async Task GetExchangeRate_GBP_ReturnsGBPExchangeRate()
        {
            //Act
            var exchangeRate = await mockConverterSVC.GetExchangeRate(ConvertSvc.Currency.GBP);

            //Assert
            var expectedRate = 52605.267;
            Assert.Equal(expectedRate,exchangeRate);
        }

        [Theory]
        [InlineData(ConvertSvc.Currency.USD,1,66812.726)]
        [InlineData(ConvertSvc.Currency.USD,2, 133625.45199999999)]
        [InlineData(ConvertSvc.Currency.EUR,1, 61472.718999999997)]
        [InlineData(ConvertSvc.Currency.EUR,2, 122945.43799999999)]
        [InlineData(ConvertSvc.Currency.GBP,1, 52605.267)]
        [InlineData(ConvertSvc.Currency.GBP,2, 105210.534)]
        public async Task ConvertBitcoins_BitCoinToCurrency_ReturnsCurrency(ConvertSvc.Currency currency, int coins, double convertedCurrency)
        {
            // When
            var converted = await mockConverterSVC.ConvertBitcoins(currency,coins);
            // Then
            Assert.Equal(convertedCurrency,converted);
        }

        [Fact]
        public async void ConvertBitcoins_BitcoinAPIServiceUnavailable_ReturnsNegativeOne()
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = new StringContent("Service not available")
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);

            var converter = new ConvertSvc(httpClient);

            //act
            var ammount = await converter.ConvertBitcoins(ConvertSvc.Currency.USD, 5);

            //Assert
            var expected = -1;
            Assert.Equal(expected, ammount);

        }

        [Fact]
        public async void ConvertBitcoin_BitcoinLeassThanZero_ThrowsArgumentExeption()
        {
            async Task result()=> await mockConverterSVC.ConvertBitcoins(ConvertSvc.Currency.USD, -6);

            await Assert.ThrowsAsync<ArgumentException>(result);
        }
    }
}
