//using Currencies.Data.Entities;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Currencies.Data
//{
//    public class SeedDatabase
//    {
//        public static void Initialize(IServiceProvider serviceProvider)
//        {
//            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
//            var logger = serviceProvider.GetRequiredService<ILogger<SeedDatabase>>();

//            var defaultCurrencies = new List<string> { "dolar", "model" };
//            var hasCurrencies = context.Currencies.Any(x => defaultCurrencies.Contains(x.Name));
//            if (!hasCurrencies)
//            {
//                context.Currencies.Add(new Currency
//                {
//                    Name = "dolar",
//                    Source = "http://www.bancoprovincia.com.ar/Principal/Dolar",
//                    MonthlyLimit = 200
//                });

//                context.Currencies.Add(new Currency
//                {
//                    Name = "real",
//                    Source = "http://www.bancoprovincia.com.ar/Principal/Dolar",
//                    MultiplyBy = 0.25,
//                    MonthlyLimit = 300
//                });
//                context.SaveChanges();
//            } 
//        }
//    }
//}
