using System;
using System.Runtime.Serialization;

namespace Pokedex.Services.FunTranslations
{
	[Serializable]
	public class TranslationServiceException : Exception
	{
		private const string MESSAGE = "Pokedex is experiencing problems with Translation service provider";
		public TranslationServiceException()
			: base(MESSAGE)
		{
		}

		public TranslationServiceException(string message)
			: base(message)
		{
		}

		public TranslationServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public TranslationServiceException(Exception innerException)
			: base(MESSAGE, innerException)
		{
		}

		protected TranslationServiceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}