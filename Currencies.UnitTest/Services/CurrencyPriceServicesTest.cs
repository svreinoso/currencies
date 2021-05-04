using Currencies.Data;
using Currencies.Data.Dtos;
using Currencies.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Currencies.UnitTest.Services
{
    public class CurrencyPriceServicesTest
    {
        private CurrencyPriceServices _currencyPriceServices;
        private ApplicationDbContext _db;
        Mock<ILogger<CurrencyPriceServices>> logger;
        private string url = "https://www.bancoprovincia.com.ar/Principal/Dolar";

        [SetUp]
        public void Setup()
        {
            _db = SetTestDb.CreateContextForInMemory();
            logger = new Mock<ILogger<CurrencyPriceServices>>();
        }

        private void SetService(HttpResponseMessage httpResponseMessage)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            _currencyPriceServices = new CurrencyPriceServices(mockFactory.Object, _db, logger.Object);
        }

        [Test]
        public async Task ParseResponseAsync()
        {
            SetService(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[\"92.500\",\"98.500\",\"Actualizada al 3 / 5 / 2021 13:00\"]"),
            });
            var result = await _currencyPriceServices.GetPriceByCurrencyName(url);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), typeof(CurrencyPriceDto));
        }

        [Test]
        public async Task CannotParseResponseAsync()
        {
            SetService(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("not valid response"),
            });
            var result = await _currencyPriceServices.GetPriceByCurrencyName(url);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetErrorResponseResponseAsync()
        {
            SetService(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("not valid response"),
            });
            var result = await _currencyPriceServices.GetPriceByCurrencyName(url);
            Assert.IsNull(result);
        }
    }
}