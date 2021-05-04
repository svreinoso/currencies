using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Currencies.Data;
using Currencies.Data.Entities;
using Currencies.Services;
using Currencies.Data.Dtos;

namespace Currencies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyPricesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrencyPriceServices _currencyPriceServices;

        public CurrencyPricesController(ApplicationDbContext context, ICurrencyPriceServices currencyPriceServices)
        {
            _context = context;
            _currencyPriceServices = currencyPriceServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrenciesPrices()
        {
            var currencies = await _context.Currencies.Where(x => !x.Disabled).ToListAsync();
            var result = new List<CurrencyPriceDto>();
            foreach(var currency in currencies)
            {
                var currencyPrice = await _currencyPriceServices.GetPriceByCurrencyName(currency.Source);
                currencyPrice.Name = currency.Name;
                result.Add(currencyPrice);
            }
            return new OkObjectResult(result);
        }

        [HttpGet("Currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await _context.Currencies.Where(x => !x.Disabled).ToListAsync();
            return new OkObjectResult(currencies);
        }

        [HttpGet("{currencyName}")]
        public async Task<IActionResult> GetCurrencyPrice(string currencyName)
        {
            return await _currencyPriceServices.GetCurrencyPrice(currencyName);
        }

    }
}
