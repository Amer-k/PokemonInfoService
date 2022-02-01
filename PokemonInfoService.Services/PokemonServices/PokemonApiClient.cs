using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokemonInfoService.Models.PokemonModels;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.PokemonServices
{
    public interface IPokemonApiClient
    {
        Task<Pokemon> SendAsync(string pokemonName);
    }

    public class PokemonApiClient : IPokemonApiClient
    {
        private const string PokemonSpeciesEndpoint = "pokemon-species";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PokemonApiClient> _logger;

        public PokemonApiClient(IHttpClientFactory httpClientFactory, ILogger<PokemonApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Pokemon> SendAsync(string pokemonName)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("PokemonApi");
                var response = await client.GetAsync($"{PokemonSpeciesEndpoint}/{pokemonName}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Pokemon>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    _logger.LogError($"Pokemon Api did not return a successful response, response code was {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while calling Pokemon Api");
                return null;
            }
        }
    }
}
