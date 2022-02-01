using PokemonInfoService.API.Models;
using PokemonInfoService.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokemonInfoService.Services.Mappers
{
    public interface IPokemonApiModelMapper
    {
        PokemonApiModel Map(Pokemon pokemon);
    }
    public class PokemonApiModelMapper : IPokemonApiModelMapper
    {
        public PokemonApiModel Map(Pokemon pokemon)
        {
            if (pokemon != null)
            {
                return new PokemonApiModel
                {
                    Name = pokemon.Name,
                    Description = GetPokemonDescription(pokemon),
                    Habitat = pokemon.Habitat?.Name,
                    IsLegendary = pokemon.Is_Legendary,
                    IsMythical = pokemon.Is_Mythical        
                };
            }

            return null;
        }

        private string GetPokemonDescription(Pokemon pokemon)
        {
            var description = pokemon.Flavor_Text_Entries.FirstOrDefault(f => f.Language.Name == "en")?.Flavor_Text;
            if (description != null)
            {
                return Regex.Replace(description, @"\t|\n|\r", " ");
            }

            return null;

        }
    }
}
