using System.IO;
using System.IO.Abstractions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.Providers;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Shouldly;
using Xunit;

namespace FoodTruckLocator.Tests.Providers
{
    public class BlobProviderTests
    {
        [Theory]
        [InlineAutoData]
        public async Task DownloadFileIfNotExists(
            [Frozen] Mock<HttpMessageHandler> messageHandlerMock,
            [Frozen] Mock<IFile> fileMock)
        {
            var options = Options.Create(new AppOptions
            {
                CsvUrl = "http://localhost"
            });
            var client = SetupHttpClient(messageHandlerMock);
            fileMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            fileMock.Setup(x => x.OpenWrite(It.IsAny<string>())).Returns(new MemoryStream());

            var sut = new BlobProvider(client, fileMock.Object, options);

            await using var stream = await sut.GetCsvAsync();

            stream.ShouldNotBeNull();

            fileMock.Verify(x => x.OpenWrite(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineAutoData]
        public async Task DoNotDownloadFileIfExists([Frozen] Mock<IFile> fileMock)
        {
            var options = Options.Create(new AppOptions());
            var client = new HttpClient();
            fileMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MemoryStream());

            var sut = new BlobProvider(client, fileMock.Object, options);

            await using var stream = await sut.GetCsvAsync();

            stream.ShouldNotBeNull();

            fileMock.Verify(x => x.OpenWrite(It.IsAny<string>()), Times.Never);
        }

        private static HttpClient SetupHttpClient(Mock<HttpMessageHandler> messageHandlerMock)
        {
            messageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(File.OpenRead("Data/Mobile_Food_Facility_Permit.csv"))
                });
            return new HttpClient(messageHandlerMock.Object);
        }
    }
}
