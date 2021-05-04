namespace Currencies.Data.Dtos
{
    public class CurrencyPriceDto
    {
        public string Name { get; set; }
        public bool Disabled { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }
        public string LastUpdateDate { get; set; }
    }
}
