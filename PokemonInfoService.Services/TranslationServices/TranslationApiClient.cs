using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokemonInfoService.Models.TranslateModels;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.TranslationServices
{
    public interface ITranslationApiClient
    {
        Task<Translation> SendAsync(string text, TranslationType translationType);
    }

    public class TranslationApiClient : ITranslationApiClient
    {
        private const string ShakespeareEndpoint = "shakespeare";
        private const string YodaEndpoint = "yoda";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TranslationApiClient> _logger;

        public TranslationApiClient(IHttpClientFactory httpClientFactory, ILogger<TranslationApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }        

        public async Task<Translation> SendAsync(string text, TranslationType translationType)
        {
            try
            {
                string endpoint;

                switch (translationType)
                {
                    case TranslationType.Shakespeare:
                        endpoint = ShakespeareEndpoint;
                        break;
                    case TranslationType.Yoda:
                        endpoint = YodaEndpoint;
                        break;
                    default:
                        endpoint = ShakespeareEndpoint;
                        break;
                }

                var client = _httpClientFactory.CreateClient("TranslationApi");
                var response = await client.GetAsync($"{endpoint}?text={text}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Translation>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    _logger.LogError($"Translation Api did not return a successful response, response code was {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while calling Translation Api");
                return null;
            }
        }
    }
}
