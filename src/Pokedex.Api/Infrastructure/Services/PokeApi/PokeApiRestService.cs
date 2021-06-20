using Pokedex.Services.PokeApi;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex
{
	public class PokeApiRestService : IPokeApiService
	{
		private readonly HttpClient _httpClient;

		public PokeApiRestService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<PokemonSpecieResponse> GetValidSpecieAsync(string name)
		{
			HttpResponseMessage response = null;
			PokemonSpecieResponse result;
			try
			{
				response = await _httpClient.GetAsync($"pokemon-species/{name}");
				response.EnsureSuccessStatusCode();
				var contentStream = await response.Content.ReadAsStreamAsync();
				result = await JsonSerializer.DeserializeAsync<PokemonSpecieResponse>(contentStream);
			}
			catch (HttpRequestException e) when (response?.StatusCode == HttpStatusCode.NotFound)
			{
				throw new SpecieNotFoundException(e, name);
			}
			catch (Exception e)
			{
				throw new PokemonServiceException(e);
			}

			if (string.IsNullOrEmpty(result.Habitat?.Name))
			{
				throw new PokemonServiceException($"Pokemon '{name}' is missing habitat information.");
			}


			if (result.FlavorTextEntries == null || result.GetFirstEnglishFlavorTextEntry() == null)
			{
				throw new PokemonServiceException($"Pokemon '{name}' is missing english description.");
			}

			return result;
		}
	}
}