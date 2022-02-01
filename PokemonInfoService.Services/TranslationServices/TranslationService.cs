using PokemonInfoService.API.Models;
using PokemonInfoService.Models.TranslateModels;
using System;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.TranslationServices
{
    public interface ITranslationService
    {
        Task<PokemonApiModel> TranslateDescription(PokemonApiModel pokemon);
    }

    public class TranslationService : ITranslationService
    {
        private const string CaveHabitate = "cave";

        private readonly ITranslationApiClient _translationApiClient;

        public TranslationService(ITranslationApiClient translationApiClient)
        {
            _translationApiClient = translationApiClient;
        }

        public async Task<PokemonApiModel> TranslateDescription(PokemonApiModel pokemon)
        {
            var translationType = (pokemon.Habitat == CaveHabitate || pokemon.IsLegendary) ? TranslationType.Yoda : TranslationType.Shakespeare;

            var translation = await _translationApiClient.SendAsync(pokemon.Description, translationType);

            if (translation != null && translation.Success.Total > 0)
            {
                pokemon.Description = translation.Contents.Translated;
            }

            // if no translation found or an error occured, return original object
            return pokemon;
        }
    }
}
