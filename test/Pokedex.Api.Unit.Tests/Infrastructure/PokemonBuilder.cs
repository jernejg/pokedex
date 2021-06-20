using System.Collections.Generic;

namespace Pokedex.Unit.Tests
{
	public static class PokemonBuilder
	{
		public static PokemonSpecieResponse BuildNoEnglishDescriptionPokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.FlavorTextEntries[0].Language.Name = "es";
			return pokemon;
		}
		public static PokemonSpecieResponse BuildNullFlavorTextEntriesPokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.FlavorTextEntries = null;
			return pokemon;
		}

		public static PokemonSpecieResponse BuildNullHabitatPokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.Habitat = null;
			return pokemon;
		}

		public static PokemonSpecieResponse BuildNullHabitatNamePokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.Habitat.Name = null;
			return pokemon;
		}

		public static PokemonSpecieResponse BuildLegendaryNonCavePokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.Name = "mewtwo";
			pokemon.IsLegendary = true;
			pokemon.Habitat.Name = "rare";
			return pokemon;
		}

		public static PokemonSpecieResponse BuildLegendaryCavePokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.Name = "legendarycavepokemon";
			pokemon.IsLegendary = true;
			pokemon.Habitat.Name = "cave";
			return pokemon;
		}

		public static PokemonSpecieResponse BuildNonLegendaryCavePokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.Name = "nonlegendarycavepokemon";
			pokemon.IsLegendary = false;
			pokemon.Habitat.Name = "cave";
			return pokemon;
		}

		public static PokemonSpecieResponse BuildNonLegendaryNonCavePokemon()
		{
			var pokemon = BuildPokemon();
			pokemon.Name = "bulbasaur";
			pokemon.IsLegendary = false;
			pokemon.Habitat.Name = "grassland";
			return pokemon;
		}

		public static PokemonSpecieResponse BuildPokemon()
		{
			return new PokemonSpecieResponse
			{
				Name = "defaultName",
				Habitat = new Habitat()
				{
					Name = "defaultHabitat"
				},
				FlavorTextEntries = new List<FlavorTextEntry>()
				{
					new FlavorTextEntry()
					{
						FlavorText="original description",
						Language=new Language()
						{
							Name="en"
						}
					}
				}
			};
		}
	}
}
