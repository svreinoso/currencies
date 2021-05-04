using Currencies.Data;
using Currencies.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Currencies.Services
{
    public interface ICurrencyPriceServices
    {
        Task<CurrencyPriceDto> GetPriceByCurrencyName(string source);
        Task<ObjectResult> GetCurrencyPrice(string currencyName);
    }

    public class CurrencyPriceServices : ICurrencyPriceServices
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CurrencyPriceServices> _logger;

        public CurrencyPriceServices(IHttpClientFactory clientFactory, ApplicationDbContext context, ILogger<CurrencyPriceServices> logger)
        {
            _clientFactory = clientFactory;
            _context = context;
            _logger = logger;
        }

        public async Task<ObjectResult> GetCurrencyPrice(string currencyName)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Name == currencyName);
            if (currency == null)
            {
                return new BadRequestObjectResult(new ApiResponseDto
                {
                    Success = false,
                    Message = "Currency not supported"
                });
            }

            var currencyPrice = await GetPriceByCurrencyName(currency.Source);

            if (currencyPrice == null)
            {
                return new BadRequestObjectResult(new ApiResponseDto
                {
                    Success = false,
                    Message = "Could not get the currency price"
                });
            }

            if (currency.MultiplyBy.HasValue)
            {
                currencyPrice.Sell *= currency.MultiplyBy.Value;
                currencyPrice.Buy *= currency.MultiplyBy.Value;
            }
            currencyPrice.Name = currency.Name;
            return new OkObjectResult(currencyPrice);
        }

        public async Task<CurrencyPriceDto> GetPriceByCurrencyName(string source)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, source);
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<List<string>>(responseStream);
                    if (result != null && result.Count == 3)
                    {
                        return new CurrencyPriceDto
                        {
                            Buy = double.Parse(result[0]),
                            Sell = double.Parse(result[1]),
                            LastUpdateDate = result[2]
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var ErrMsg = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();
                    _logger.LogError("Error getting currency price: " + ErrMsg);
                    return null;
                }
            } catch(Exception ex)
            {
                _logger.LogError("Error getting currency price: " + ex.Message);
                return null;
            }
        }
    }
}
