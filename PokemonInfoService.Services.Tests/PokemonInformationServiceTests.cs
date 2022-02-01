using Moq;
using NUnit.Framework;
using PokemonInfoService.API.Models;
using PokemonInfoService.Models;
using PokemonInfoService.Services.Mappers;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.Tests
{
    public class PokemonInformationServiceTests
    {
        private IPokemonInformationService _pokemonInformationService;

        private Mock<IPokemonApiClient> _pokemonApiClient;
        private Mock<IPokemonApiModelMapper> _pokemonApiModelMapper;

        [SetUp]
        public void SetUp()
        {
            _pokemonApiClient = new Mock<IPokemonApiClient>();
            _pokemonApiModelMapper = new Mock<IPokemonApiModelMapper>();

            _pokemonInformationService = new PokemonInformationService(_pokemonApiClient.Object, _pokemonApiModelMapper.Object);
        }

        [Test]
        public async Task GetPokemonInformationAsync_ReturnsPokemonApiModel()
        {
            // Arrange
            var pokemonName = "mewtwo";
            var pokemonModel = new Pokemon()
            {
                Name = pokemonName
            };
            var pokemonApiModel = new PokemonApiModel()
            {
                Name = pokemonName
            };

            _pokemonApiClient.Setup(c => c.SendAsync(pokemonName)).Returns(Task.FromResult(pokemonModel));
            _pokemonApiModelMapper.Setup(m => m.Map(pokemonModel)).Returns(pokemonApiModel);

            // Act
            var result = await _pokemonInformationService.GetPokemonInformationAsync(pokemonName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(pokemonName, result.Name);
            Assert.IsNull(result.Error);
        }

        [Test]
        public async Task GetPokemonInformationAsync_RequestFails_ReturnsErrorPokemonApiModel()
        {
            // Arrange
            var pokemonName = "mewtwo";            

            _pokemonApiClient.Setup(c => c.SendAsync(pokemonName)).Returns(Task.FromResult(default(Pokemon)));            

            // Act
            var result = await _pokemonInformationService.GetPokemonInformationAsync(pokemonName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual($"There was an error retrieving information for pokemon {pokemonName}", result.Error);
        }
    }
}
