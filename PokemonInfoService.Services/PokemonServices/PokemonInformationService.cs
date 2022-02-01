using PokemonInfoService.API.Models;
using PokemonInfoService.Services.Mappers;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.PokemonServices
{
    public interface IPokemonInformationService
    {
        Task<PokemonApiModel> GetPokemonInformationAsync(string pokemonName);
    }
    
    public class PokemonInformationService : IPokemonInformationService
    {        
        private readonly IPokemonApiClient _pokemonApiclient;
        private readonly IPokemonApiModelMapper _pokemonApiModelMapper;


        public PokemonInformationService(IPokemonApiClient pokemonApiclient, IPokemonApiModelMapper pokemonApiModelMapper)
        {
            _pokemonApiclient = pokemonApiclient;
            _pokemonApiModelMapper = pokemonApiModelMapper;
        }

        public async Task<PokemonApiModel> GetPokemonInformationAsync(string pokemonName)
        {
            var pokemon = await _pokemonApiclient.SendAsync(pokemonName);

            if (pokemon == null)
            {
                return new PokemonApiModel()
                {
                    Error = $"There was an error retrieving information for pokemon {pokemonName}"
                };
            }

            return _pokemonApiModelMapper.Map(pokemon);
        }
    }
}
