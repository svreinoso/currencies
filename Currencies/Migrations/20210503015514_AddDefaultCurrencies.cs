using Currencies.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Currencies.Migrations
{
    public partial class AddDefaultCurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[Currencies]
                    ([Name]
                    ,[Source]
                    ,[MultiplyBy]
                    ,[MonthlyLimit]
                    ,[Disabled]
                    ,[CreatedDate]
                    ,[UpdatedDate])
                VALUES
                    ('dolar', 'http://www.bancoprovincia.com.ar/Principal/Dolar', NULL, 200, 0, GETDATE(), GETDATE() ),
                    ('real', 'http://www.bancoprovincia.com.ar/Principal/Dolar', 0.25, 300, 0, GETDATE(), GETDATE() )
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
