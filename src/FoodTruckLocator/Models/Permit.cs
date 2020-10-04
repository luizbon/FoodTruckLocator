using System;
using CsvHelper.Configuration.Attributes;

namespace FoodTruckLocator.Models
{
    public class Permit
    {
        [Name("locationid")]
        public string LocationId { get; set; }
        [Name("Applicant")]
        public string Name { get; set; }
        [Name("Address")]
        public string Address { get; set; }
        [Name("FoodItems")]
        public string FoodItems { get; set; }
        [Name("Latitude")]
        public double Latitude { get; set; }
        [Name("Longitude")]
        public double Longitude { get; set; }
        [Name("dayshours")]
        public string DaysHours { get; set; }
        [Name("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }
    }
}
