using Pokedex.Services.FunTranslations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Unit.Tests.Infrastructure
{
	public static class TranslationBuilder
	{
		public static TranslateResponse BuildTranslation()
		{
			return new TranslateResponse()
			{
				Success = new Success() { Total = 1 },
				Contents = new Contents()
				{
					Translated = "translated text",
				}
			};
		}
	}
}
