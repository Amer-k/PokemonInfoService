using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PokemonInfoService.Services.PokemonServices;
using PokemonInfoService.Services.Tests.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.Tests.PokemonServices
{
    public class PokemonApiClientTest
    {
        private IPokemonApiClient _pokemonApiClient;

        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private HttpClient _httpClientMock;
        private FakeHttpMessageHandler _fakeHttpMessageHandler;
        private Mock<ILogger<PokemonApiClient>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);

            _fakeHttpMessageHandler = new FakeHttpMessageHandler();
            _httpClientMock = new HttpClient(_fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("https://pokemon")
            };
            
            _httpClientFactoryMock.Setup(k => k.CreateClient("PokemonApi"))
               .Returns(_httpClientMock);
            
            _loggerMock = new Mock<ILogger<PokemonApiClient>>();

            _pokemonApiClient = new PokemonApiClient(_httpClientFactoryMock.Object, _loggerMock.Object);            
        }

        [Test]
        public async Task SendAsync_SuccessfulResponse_ReturnsResponse()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage()
            {
                Content = new StringContent("{\"name\":\"mewtwo\"}")
            };

            _fakeHttpMessageHandler.HttpResponseMessage = responseMessage;

            // Act
            var response = await _pokemonApiClient.SendAsync("mewtwo");

            // Assert
            Assert.NotNull(response);
            Assert.AreEqual("mewtwo", response.Name);
        }

        [Test]
        public async Task SendAsync_RequestErrors_ReturnsFailedResponse()
        {
            // Arrange
            _fakeHttpMessageHandler.HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            // Act
            var response = await _pokemonApiClient.SendAsync("mewtwo");

            // Assert
            Assert.AreEqual(null, response);
        }
    }
}
