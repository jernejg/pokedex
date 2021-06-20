using System;
using System.Linq;

namespace Pokedex
{
	public static class Helpers
	{
		public static FlavorTextEntry GetFirstEnglishFlavorTextEntry(this PokemonSpecieResponse me)
		{
			return me.FlavorTextEntries
						.Where(x => string.Compare("en", x.Language.Name, StringComparison.OrdinalIgnoreCase) == 0)
						.FirstOrDefault();
		}
	}
}
