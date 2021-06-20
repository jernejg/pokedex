using System;
using System.Runtime.Serialization;

namespace Pokedex
{
	[Serializable]
	public class PokemonServiceException : Exception
	{
		private const string MESSAGE = "Pokedex is experiencing problems with Pokemon data provider";
		public PokemonServiceException()
			: base(MESSAGE)
		{
		}

		public PokemonServiceException(string message)
			: base(message)
		{
		}

		public PokemonServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public PokemonServiceException(Exception innerException)
			: base(MESSAGE, innerException)
		{
		}

		protected PokemonServiceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}