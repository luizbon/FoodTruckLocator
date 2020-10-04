using System;
using System.IO;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;
using FoodTruckLocator.Configuration;
using Microsoft.Extensions.Options;

namespace FoodTruckLocator.Providers
{
    public interface IBlobProvider
    {
        Task<Stream> GetCsvAsync();
        Task DownloadCsvAsync();
    }

    public class BlobProvider : IBlobProvider
    {
        private const string CsvPath = "Data/Mobile_Food_Facility_Permit.csv";
        private readonly HttpClient _httpClient;
        private readonly IFile _file;
        private readonly AppOptions _appOptions;

        public BlobProvider(HttpClient httpClient, IFile file, IOptions<AppOptions> options)
        {
            _httpClient = httpClient;
            _file = file;
            _appOptions = options.Value;
        }

        public async Task<Stream> GetCsvAsync()
        {
            if (!_file.Exists(CsvPath))
                await DownloadCsvAsync();

            return _file.OpenRead(CsvPath);
        }

        public async Task DownloadCsvAsync()
        {
            var response = await _httpClient.GetAsync(_appOptions.CsvUrl);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            await WriteFile(stream);
        }

        private async Task WriteFile(Stream stream, int retryCount = 0)
        {
            try
            {
                stream.Position = 0;
                using var writeStream = _file.OpenWrite(CsvPath);
                await stream.CopyToAsync(writeStream);
            }
            catch
            {
                if (retryCount > 5)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromMilliseconds(Math.Pow(2, retryCount)));

                await WriteFile(stream, ++retryCount);
            }
        }
    }
}