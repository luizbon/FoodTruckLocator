using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBogus;
using AutoBogus.Conventions;
using AutoBogus.Moq;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.Models;
using FoodTruckLocator.Providers;
using FoodTruckLocator.Services;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace FoodTruckLocator.Tests.Services
{
    public class SearchServiceTests: IClassFixture<SearchServiceFixture>
    {
        private readonly SearchServiceFixture _fixture;

        public SearchServiceTests(SearchServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(0,0,5)]
        [InlineData(0,0,null)]
        public async Task FindPermitsTests(double lat, double lng, int? resultSize)
        {
            var storageProvider = new Mock<IStorageProvider>();
            storageProvider.Setup(x => x.ReadPermitsAsync()).ReturnsAsync(_fixture.GetPermits(10));
            var options = Options.Create(new AppOptions
            {
                NumberOfResults = 5
            });

            var sut = new SearchService(storageProvider.Object, options);

            var result = await sut.FindAsync(lat, lng, resultSize);

            result.Count().ShouldBe(resultSize ?? 5);
        }
    }

    public class SearchServiceFixture
    {
        private readonly AutoFaker<Permit> _generator;

        public SearchServiceFixture()
        {
            _generator = new AutoFaker<Permit>();
            _generator.Configure(builder =>
            {
                builder.WithConventions();
                builder.WithBinder<MoqBinder>();
            });
        }

        public IEnumerable<Permit> GetPermits(int count)
        {
            return _generator.GenerateLazy(count);
        }
    }
}
