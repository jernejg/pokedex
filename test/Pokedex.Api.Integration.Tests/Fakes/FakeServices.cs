using Pokedex.Services.FunTranslations;
using Pokedex.Services.PokeApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokedex.Integration.Tests.Fakes
{
	public class FakePokeApiService : IPokeApiService
	{
		private readonly bool _shouldThrowSpecieNotFound;
		private readonly bool _shouldThrowPokeApiServiceException;

		public FakePokeApiService(bool shouldThrowSpecieNotFound = false, bool shouldThrowPokeApiServiceException = false)
		{
			_shouldThrowSpecieNotFound = shouldThrowSpecieNotFound;
			_shouldThrowPokeApiServiceException = shouldThrowPokeApiServiceException;
		}
		public Task<PokemonSpecieResponse> GetValidSpecieAsync(string name)
		{
			if (_shouldThrowSpecieNotFound)
				throw new SpecieNotFoundException();

			if (_shouldThrowPokeApiServiceException)
				throw new PokemonServiceException();

			return Task.FromResult(new PokemonSpecieResponse()
			{
				Name = name,
				Habitat = new Habitat() { Name = "cave" },
				IsLegendary = true,
				FlavorTextEntries = new List<FlavorTextEntry>() { new FlavorTextEntry() { FlavorText = "desc", Language = new Language() { Name = "en" } } }
			});
		}
	}

	public class FakeFunTranslationService : IFunTranslationsService
	{
		private readonly bool _shouldThrowTranslationServiceException;

		public FakeFunTranslationService(bool shouldThrowTranslationServiceException = false)
		{
			_shouldThrowTranslationServiceException = shouldThrowTranslationServiceException;
		}
		public Task<TranslateResponse> GetShakespeareTranslationsAsync(string text)
		{
			if (_shouldThrowTranslationServiceException)
				throw new TranslationServiceException();

			return Task.FromResult(new TranslateResponse()
			{
				Success = new Success()
				{
					Total = 1
				},
				Contents = new Contents()
				{
					Text = text,
					Translated = "translated",
					Translation = "shakespeare"
				}
			});
		}

		public Task<TranslateResponse> GetYodaTranslationsAsync(string text)
		{
			if (_shouldThrowTranslationServiceException)
				throw new TranslationServiceException();
			return Task.FromResult(new TranslateResponse()
			{
				Success = new Success()
				{
					Total = 1
				},
				Contents = new Contents()
				{
					Text = text,
					Translated = "translated",
					Translation = "yoda"
				}
			});
		}
	}
}
