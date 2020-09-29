using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTruckLocator.Models
{
    public class Permit
    {
        public string LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FoodItems { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DaysHours { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
