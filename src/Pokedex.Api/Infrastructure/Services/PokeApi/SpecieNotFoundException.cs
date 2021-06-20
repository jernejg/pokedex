using System;
using System.Runtime.Serialization;

namespace Pokedex.Services.PokeApi
{
	[Serializable]
	public class SpecieNotFoundException : Exception
	{
		public SpecieNotFoundException()
		{
		}

		public SpecieNotFoundException(string message)
			: base(message)
		{
		}

		public SpecieNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected SpecieNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public SpecieNotFoundException(Exception inner, string pokemonName)
			: base($"Specie '{pokemonName}' does not exist.", inner)
		{
		}
	}
}
