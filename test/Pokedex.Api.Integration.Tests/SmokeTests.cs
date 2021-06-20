using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api;
using Pokedex.Integration.Tests.Fakes;
using System;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Integration.Tests
{
	public class SmokeTests : IClassFixture<WebApplicationFactory<Startup>>
	{
		private readonly WebApplicationFactory<Startup> _factory;

		public SmokeTests(WebApplicationFactory<Startup> factory)
		{
			_factory = factory;
			_factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/v1/");
		}

		[Theory]
		[InlineData(Helpers.BASIC_ENDPOINT, "mewtwo", "desc")]
		public async Task Happy_Path_Test(string endpoint, string pokeName, string expectedDescription)
		{
			var fakePokeService = new FakePokeApiService();
			var expectedModel = await fakePokeService.GetValidSpecieAsync(pokeName);
			
			var client = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton<IPokeApiService>(fakePokeService);
				});
			}).CreateClient();

			var actualModel = await client.GetFromJsonAsync<ExpectedModel>($"{endpoint}/{pokeName}");

			Assert.NotNull(actualModel);
			Assert.Equal(expectedModel.Name, actualModel.Name);
			Assert.Equal(expectedModel.IsLegendary, actualModel.IsLegendary);
			Assert.Equal(expectedDescription, actualModel.Description);
			Assert.Equal(expectedModel.Habitat.Name, actualModel.Habitat);
		}
	}

	public class ExpectedModel
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("habitat")]
		public string Habitat { get; set; }
		[JsonPropertyName("isLegendary")]
		public bool IsLegendary { get; set; }
	}
}
