using Microsoft.AspNetCore.Mvc;
using PokemonInfoService.API.Models;
using PokemonInfoService.Services;
using System.Threading.Tasks;

namespace PokemonInfoService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Pokemon : ControllerBase
    {
        private readonly IPokemonInformationService _pokemonInformationService;

        public Pokemon(IPokemonInformationService pokemonInformationService)
        {
            _pokemonInformationService = pokemonInformationService;
        }

        [HttpGet]
        public async Task<PokemonApiModel> Get(string pokemonName)
        {
            return await _pokemonInformationService.GetPokemonInformationAsync(pokemonName);
        }
    }
}
