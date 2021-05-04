using Currencies.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Currencies.Data.Entities
{
    public class Purchase : ITablesAudit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Currency is required")]
        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
        public double Quantity { get; set; }

        public double Price { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
