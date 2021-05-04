using System;

namespace Currencies.Data.Interfaces
{
    public interface ITablesAudit
    {
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset UpdatedDate { get; set; }
    }
}
