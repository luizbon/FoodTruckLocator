using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.Models;
using FoodTruckLocator.Providers;
using Geolocation;
using Microsoft.Extensions.Options;

namespace FoodTruckLocator.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<Permit>> FindAsync(double lat, double lng, int? numberOfResults);
    }

    public class SearchService : ISearchService
    {
        private readonly IStorageProvider _storageProvider;
        private readonly AppOptions _appOptions;

        public SearchService(IStorageProvider storageProvider, IOptions<AppOptions> appOptions)
        {
            _storageProvider = storageProvider;
            _appOptions = appOptions.Value;
        }

        public async Task<IEnumerable<Permit>> FindAsync(double lat, double lng, int? numberOfResults)
        {
            var permits = await _storageProvider.ReadPermitsAsync();

            return permits.Select(permit => new
                {
                    Permit = permit,
                    Distance = GeoCalculator.GetDistance(lat, lng, permit.Latitude, permit.Longitude)
                }).OrderBy(result => result.Distance)
                .Take(numberOfResults ?? _appOptions.NumberOfResults)
                .Select(result => result.Permit);
        }
    }
}
