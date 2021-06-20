using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex
{
	public class PokemonSpecieResponse
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("is_legendary")]
		public bool IsLegendary { get; set; }
		[JsonPropertyName("habitat")]
		public Habitat Habitat { get; set; }
		[JsonPropertyName("flavor_text_entries")]
		public IList<FlavorTextEntry> FlavorTextEntries { get; set; }
	}

	public class Habitat
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("url")]
		public string Url { get; set; }
	}

	public class FlavorTextEntry
	{
		[JsonPropertyName("flavor_text")]
		public string FlavorText { get; set; }
		[JsonPropertyName("language")]
		public Language Language { get; set; }
	}

	public class Language
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}