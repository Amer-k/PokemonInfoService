namespace PokemonInfoService.API.Models
{
    public class PokemonApiModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
        public bool IsMythical { get; set; }
        public string Error { get; set; }
    }
}
