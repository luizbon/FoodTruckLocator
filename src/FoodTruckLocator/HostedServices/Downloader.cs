using System;
using System.Threading;
using System.Threading.Tasks;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.Providers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FoodTruckLocator.HostedServices
{
    public class Downloader: IHostedService, IDisposable
    {
        private readonly IBlobProvider _blobProvider;
        private readonly AppOptions _appOptions;
        private Timer _timer;

        public Downloader(IBlobProvider blobProvider, IOptions<AppOptions> options)
        {
            _blobProvider = blobProvider;
            _appOptions = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DownloadFile, null, TimeSpan.Zero, TimeSpan.FromHours(_appOptions.CsvRefreshIntervalInHours));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DownloadFile(object state)
        {
            _blobProvider.DownloadCsvAsync();
        }
    }
}
