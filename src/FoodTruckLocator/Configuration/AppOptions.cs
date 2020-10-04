namespace FoodTruckLocator.Configuration
{
    public class AppOptions
    {
        public const string Section = "Application";

        public int NumberOfResults { get; set; }

        public string CsvUrl { get; set; }

        public int CsvRefreshIntervalInHours { get; set; }
    }
}
