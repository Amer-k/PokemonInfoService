using System.Collections.Generic;

namespace PokemonInfoService.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public List<FlavorText> Flavor_Text_Entries { get; set; }
        public PokemonHabitat Habitat { get; set; }
        public bool Is_Legendary { get; set; }
        public bool Is_Mythical { get; set; }
    }
}
