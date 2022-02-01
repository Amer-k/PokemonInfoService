using Microsoft.AspNetCore.Mvc;
using PokemonInfoService.API.Models;
using PokemonInfoService.Services.PokemonServices;
using PokemonInfoService.Services.TranslationServices;
using System.Threading.Tasks;

namespace PokemonInfoService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Pokemon : ControllerBase
    {
        private readonly IPokemonInformationService _pokemonInformationService;
        private readonly ITranslationService _translationService;

        public Pokemon(IPokemonInformationService pokemonInformationService, ITranslationService translationService)
        {
            _pokemonInformationService = pokemonInformationService;
            _translationService = translationService;
        }

        [HttpGet]
        public async Task<PokemonApiModel> Get(string pokemonName)
        {
            return await _pokemonInformationService.GetPokemonInformationAsync(pokemonName.ToLower());
        }

        [Route("translated")]
        [HttpGet]
        public async Task<PokemonApiModel> GetTranslatedPokemon(string pokemonName)
        {
            var pokemon = await _pokemonInformationService.GetPokemonInformationAsync(pokemonName.ToLower());
            return await _translationService.TranslateDescription(pokemon);
        }
    }
}
