using Pokedex.Services.FunTranslations;
using System.Threading.Tasks;

namespace Pokedex
{
	public interface IFunTranslationsService
	{
		Task<TranslateResponse> GetShakespeareTranslationsAsync(string text);
		Task<TranslateResponse> GetYodaTranslationsAsync(string text);
	}
}
