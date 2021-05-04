using Currencies.Data;
using Currencies.Data.Dtos;
using Currencies.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public interface IPurchaseServices
    {
        Task<ObjectResult> PostPurchase(PurchaseDto purchase);
        Task<ObjectResult> GetPurchaseByUserId(int userId);
    }

    public class PurchaseServices : IPurchaseServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrencyPriceServices _currencyPriceServices;

        public PurchaseServices(ApplicationDbContext context, ICurrencyPriceServices currencyPriceServices)
        {
            _context = context;
            _currencyPriceServices = currencyPriceServices;
        }

        public async Task<ObjectResult> GetPurchaseByUserId(int userId)
        {
            var purchases = await _context.Purchases.Include(x => x.Currency).Where(x => x.UserId == userId)
                .Select(x => new PurchaseDto
                {
                    UserId = x.UserId,
                    Amount = x.Quantity/x.Price,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    CurrencyName = x.Currency.Name
                }).ToListAsync();
            return new OkObjectResult(purchases);
        }

        public async Task<ObjectResult> PostPurchase(PurchaseDto purchaseDto)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == purchaseDto.CurrencyId);
            if (currency == null)
            {
                return new BadRequestObjectResult(new ApiResponseDto
                {
                    Success = false,
                    Message = "Currency not found"
                });
            }


            var currencyPrice = await _currencyPriceServices.GetPriceByCurrencyName(currency.Source);
            if (currencyPrice == null)
            {
                return new BadRequestObjectResult(new ApiResponseDto
                {
                    Success = false,
                    Message = "Could not get the currency price"
                });
            }
            var price = currency.MultiplyBy.HasValue ? currencyPrice.Buy * currency.MultiplyBy.Value : currencyPrice.Buy;

            var today = DateTimeOffset.Now;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var purchasesInCurrentMonth = await _context.Purchases.Where(x =>
                x.UserId == purchaseDto.UserId &&
                x.CreatedDate > firstDayOfMonth &&
                x.CreatedDate < lastDayOfMonth).SumAsync(x => x.Quantity / x.Price);

            if (purchasesInCurrentMonth + purchaseDto.Quantity / price > currency.MonthlyLimit)
            {
                return new BadRequestObjectResult(new ApiResponseDto
                {
                    Success = false,
                    Message = $"Purchase limit for this currency is {currency.MonthlyLimit} by month"
                });
            }

            var purchase = new Purchase
            {
                UserId = purchaseDto.UserId,
                Quantity = purchaseDto.Quantity,
                Price = price,
                CurrencyId = purchaseDto.CurrencyId
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new ApiResponseDto
            {
                Success = true
            });
        }
    }
}
