using Currencies.Data;
using Currencies.Data.Dtos;
using Currencies.Data.Entities;
using Currencies.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Currencies.UnitTest.Services
{
    public class PurchaseServicesTest
    {
        private PurchaseServices _purchaseServices;
        Mock<ICurrencyPriceServices> currencyServices;
        private ApplicationDbContext _db;
        private string url = "https://www.bancoprovincia.com.ar/Principal/Dolar";
        private int monthlyLimit = 200;
        private int userId = 1;
        private int currencyId = 1;

        private void AddDefaultCurrency()
        {
            _db = SetTestDb.CreateContextForInMemory();
            _db.Currencies.Add(new Currency
            {
                Name = "dolar",
                Source = url,
                MonthlyLimit = monthlyLimit
            });
            _db.SaveChanges();
        }

        private void SetupCurrencyService()
        {
            currencyServices = new Mock<ICurrencyPriceServices>();
            currencyServices.Setup(x => x.GetPriceByCurrencyName(url)).ReturnsAsync(new CurrencyPriceDto
            {
                Buy = 92.750,
                Sell = 98.750
            });
        }

        [Test]
        public async Task MakePurchaseAsync()
        {
            AddDefaultCurrency();
            SetupCurrencyService();
            _purchaseServices = new PurchaseServices(_db, currencyServices.Object);

            var result = await _purchaseServices.PostPurchase(new PurchaseDto
            {
                UserId = userId,
                CurrencyId = currencyId,
                Quantity = 100
            });
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponseDto;
            Assert.IsTrue(apiResponse.Success);
        }

        [Test]
        public async Task CannotPurchaseMoreThanMontlyLimit()
        {
            AddDefaultCurrency();
            SetupCurrencyService();
            _db.Purchases.Add(new Purchase
            {
                UserId = userId,
                CurrencyId = currencyId,
                Quantity = monthlyLimit
            });
            _db.SaveChanges();
            _purchaseServices = new PurchaseServices(_db, currencyServices.Object);
            var result = await _purchaseServices.PostPurchase(new PurchaseDto
            {
                UserId = userId,
                CurrencyId = currencyId,
                Quantity = 100
            });
            Assert.IsNotNull(result);
            var okResult = result as BadRequestObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponseDto;
            Assert.IsFalse(apiResponse.Success);
            Assert.IsTrue(apiResponse.Message == $"Purchase limit for this currency is {monthlyLimit} by month");
        }


    }
}
