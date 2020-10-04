using System.IO;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.Providers;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace FoodTruckLocator.Tests.Providers
{
    public class StorageProviderTests
    {
        [Theory]
        [InlineAutoData]
        public async Task ParseCsvToPermit([Frozen]Mock<IBlobProvider> blobMock)
        {
            var options = Options.Create(new CachingOptions
            {
                CsvFileExpirationInHours = 1
            });

            blobMock.Setup(x => x.GetCsvAsync())
                .ReturnsAsync(() => File.OpenRead("Data/Mobile_Food_Facility_Permit.csv"));

            var cacheMock = Create.MockedMemoryCache();

            var sut = new StorageProvider(blobMock.Object, cacheMock, options);
            
            var result = await sut.ReadPermitsAsync();

            result.ShouldNotBeEmpty();
            cacheMock.Get(nameof(sut.ReadPermitsAsync)).ShouldBe(result);
            blobMock.Verify(x => x.GetCsvAsync(), Times.Once);
        }
    }
}
