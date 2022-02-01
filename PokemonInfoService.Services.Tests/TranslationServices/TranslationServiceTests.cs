using Moq;
using NUnit.Framework;
using PokemonInfoService.API.Models;
using PokemonInfoService.Models.TranslateModels;
using PokemonInfoService.Services.TranslationServices;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.Tests.TranslationServices
{
    public class TranslationServiceTests
    {
        private ITranslationService _translationService;

        private Mock<ITranslationApiClient> _translationApiClientMock;

        [SetUp]
        public void SetUp()
        {
            _translationApiClientMock = new Mock<ITranslationApiClient>();
            _translationService = new TranslationService(_translationApiClientMock.Object);
        }

        [Test]        
        [TestCase("rare", true, TranslationType.Yoda)]
        [TestCase("cave", false, TranslationType.Yoda)]
        [TestCase("cave", true, TranslationType.Yoda)]
        [TestCase("forest", false, TranslationType.Shakespeare)]
        [TestCase("electric", false, TranslationType.Shakespeare)]
        [TestCase("forest", true, TranslationType.Yoda)]
        [TestCase("electric", true, TranslationType.Yoda)]
        public async Task TranslateDescription_UsesCorrectTranslationAsync(string habitat, bool isLegendry, TranslationType translationType)
        {
            // Arrange
            var pokemon = new PokemonApiModel()
            {
                Name = "Some Name",
                Description = "some description",
                Habitat = habitat,
                IsLegendary = isLegendry                
            };

            var translation = new Translation()
            {
                Success = new SuccessModel
                {
                    Total = 1
                },
                Contents = new ContentsModel
                {
                    Translated = "translated description"
                }
            };

            _translationApiClientMock.Setup(t => t.SendAsync(pokemon.Description, It.IsAny<TranslationType>()))
                .Returns(Task.FromResult(translation));

            // Act
            var translatedPokemon = await _translationService.TranslateDescription(pokemon);

            // Assert
            Assert.IsNotNull(translatedPokemon);
            Assert.AreEqual(pokemon.Name, translatedPokemon.Name);
            Assert.AreEqual(pokemon.Habitat, translatedPokemon.Habitat);
            Assert.AreEqual(translation.Contents.Translated, translatedPokemon.Description);
            _translationApiClientMock.Verify(t => t.SendAsync("some description", translationType));
        }

        [Test]
        public async Task TranslateDescription_TranslationReturnsNull_KeepsOriginalTest()
        {
            // Arrange
            var pokemon = new PokemonApiModel()
            {                
                Description = "some description",
            };

            _translationApiClientMock.Setup(t => t.SendAsync(pokemon.Description, It.IsAny<TranslationType>()))
                .Returns(Task.FromResult(default(Translation)));

            // Act
            var translatedPokemon = await _translationService.TranslateDescription(pokemon);

            // Assert
            Assert.IsNotNull(translatedPokemon);            
            Assert.AreEqual(pokemon.Description, translatedPokemon.Description);            
        }

        [Test]
        public async Task TranslateDescription_TranslationNotSuccess_KeepsOriginalTest()
        {
            // Arrange
            var pokemon = new PokemonApiModel()
            {
                Description = "some description",
            };

            var translation = new Translation()
            {
                Success = new SuccessModel
                {
                    Total = 0
                },
                Contents = new ContentsModel
                {
                    Translated = "translated description"
                }
            };

            _translationApiClientMock.Setup(t => t.SendAsync(pokemon.Description, It.IsAny<TranslationType>()))
                .Returns(Task.FromResult(translation));

            // Act
            var translatedPokemon = await _translationService.TranslateDescription(pokemon);

            // Assert
            Assert.IsNotNull(translatedPokemon);
            Assert.AreEqual(pokemon.Description, translatedPokemon.Description);
        }
    }
}
