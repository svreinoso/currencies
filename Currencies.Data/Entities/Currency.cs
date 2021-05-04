using Currencies.Data.Interfaces;
using System;

namespace Currencies.Data.Entities
{
    public class Currency : ITablesAudit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public double? MultiplyBy { get; set; }
        public int MonthlyLimit { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
