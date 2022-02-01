using NUnit.Framework;
using PokemonInfoService.Models.PokemonModels;
using PokemonInfoService.Services.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace PokemonInfoService.Services.Tests.Mappers
{
    public class PokemonApiModelMapperTests
    {
        private PokemonApiModelMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new PokemonApiModelMapper();
        }

        [Test]
        public void Map_MapsPokemonToPokemonApiModel()
        {
            var pokemonName = "mewtwo";
            var description = "test description";
            var habitat = "some habitat";
            var pokemonModel = new Pokemon()
            {
                Name = pokemonName,
                Flavor_Text_Entries = new List<FlavorText>()
                {
                    new FlavorText()
                    {
                        Language = new FlavorLanguage()
                        {
                            Name = "en"
                        },
                        Flavor_Text = description
                    }
                },
                Habitat = new PokemonHabitat() { Name = habitat },
                Is_Legendary = true,
                Is_Mythical = false
            };

            // Act
            var pokemonApiModel = _mapper.Map(pokemonModel);

            // Assert
            Assert.IsNotNull(pokemonApiModel);
            Assert.AreEqual(pokemonModel.Name, pokemonApiModel.Name);
            Assert.AreEqual(pokemonModel.Habitat.Name, pokemonApiModel.Habitat);
            Assert.AreEqual(pokemonModel.Flavor_Text_Entries.First().Flavor_Text, pokemonApiModel.Description);
            Assert.AreEqual(pokemonModel.Is_Legendary, pokemonApiModel.IsLegendary);
            Assert.AreEqual(pokemonModel.Is_Mythical, pokemonApiModel.IsMythical);
        }

        [Test]
        public void Map_MapsEnglishDescription()
        {
            var pokemonModel = new Pokemon()
            {               
                Flavor_Text_Entries = new List<FlavorText>()
                {
                    new FlavorText()
                    {
                        Language = new FlavorLanguage()
                        {
                            Name = "fr"
                        },
                        Flavor_Text = "French description"
                    },
                    new FlavorText()
                    {
                         Language = new FlavorLanguage()
                        {
                            Name = "en"
                        },
                        Flavor_Text = "English description"
                    }
                }
            };

            // Act
            var pokemonApiModel = _mapper.Map(pokemonModel);

            // Assert
            Assert.IsNotNull(pokemonApiModel);          
            Assert.AreEqual("English description", pokemonApiModel.Description);
        }

        [Test]
        public void Map_MapsNull_ReturnNull()
        {         
            // Act
            var pokemonApiModel = _mapper.Map(default(Pokemon));

            // Assert
            Assert.IsNull(pokemonApiModel);            
        }
    }
}
