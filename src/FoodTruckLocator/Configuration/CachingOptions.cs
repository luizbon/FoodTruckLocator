namespace FoodTruckLocator.Configuration
{
    public class CachingOptions
    {
        public const string Section = "Caching";

        public int CsvFileExpirationInHours { get; set; }
    }
}
