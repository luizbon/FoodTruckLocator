using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FoodTruckLocator.Providers
{
    public interface IStorageProvider
    {
        Task<IEnumerable<Permit>> ReadPermitsAsync();
    }

    public class StorageProvider:IStorageProvider
    {
        private readonly IBlobProvider _blobProvider;
        private readonly IMemoryCache _cache;
        private readonly CachingOptions _options;

        public StorageProvider(IBlobProvider blobProvider, IMemoryCache cache, IOptions<CachingOptions> cachingOptions)
        {
            _blobProvider = blobProvider;
            _cache = cache;
            _options = cachingOptions.Value;
        }

        public async Task<IEnumerable<Permit>> ReadPermitsAsync()
        {
            return await _cache.GetOrCreateAsync(nameof(ReadPermitsAsync), ReadCsv);
        }

        private async Task<IEnumerable<Permit>> ReadCsv(ICacheEntry entry)
        {
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(_options.CsvFileExpirationInHours);
            using var csvFile = await _blobProvider.GetCsvAsync();
            using var reader = new StreamReader(csvFile);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Permit>().ToList();
        }
    }
}
