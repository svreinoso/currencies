using System;
using System.ComponentModel.DataAnnotations;

namespace Currencies.Data.Dtos
{
    public class PurchaseDto
    {
        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "{0}required")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} required")]
        public int UserId { get; set; }

        public double Amount { get; set; }

        public double Price { get; set; }

        public string CurrencyName { get; set; }
    }
}
