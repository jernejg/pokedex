using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Services.FunTranslations
{
	public class FunTranslationsRestApiService : IFunTranslationsService
	{
		private readonly HttpClient _httpClient;
		public FunTranslationsRestApiService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<TranslateResponse> GetYodaTranslationsAsync(string text)
		{
			return await getTranslationsAsync("yoda", text);
		}
		public async Task<TranslateResponse> GetShakespeareTranslationsAsync(string text)
		{
			return await getTranslationsAsync("shakespeare", text);
		}
		/*
			502 – The server while acting as a gateway or a proxy, 
			received an invalid response from the upstream server it accessed
			in attempting to fulfill the request.

			504 – The server, while acting as a gateway or proxy,
			did not receive a timely response from the upstream server
			specified by the URI(e.g.HTTP, FTP, LDAP)
			or some other auxiliary server(e.g.DNS) it needed to access 
			in attempting to complete the request.
		*/
		private async Task<TranslateResponse> getTranslationsAsync(string endpoint, string text)
		{
			TranslateResponse translateResponse;
			try
			{
				var values = new Dictionary<string, string> { { "text", text } };
				var response = await _httpClient.PostAsync(endpoint, new FormUrlEncodedContent(values));
				response.EnsureSuccessStatusCode();
				var contentStream = await response.Content.ReadAsStreamAsync();
				translateResponse = await JsonSerializer.DeserializeAsync<TranslateResponse>(contentStream);
			}
			catch (Exception e)
			{
				throw new TranslationServiceException(e);
			}


			if (translateResponse.Success?.Total != 1)
			{
				throw new TranslationServiceException();
			}

			return translateResponse;
		}
	}
}
