using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api;
using Pokedex.Integration.Tests.Fakes;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Integration.Tests
{
	public class MiddlewareTests : IClassFixture<WebApplicationFactory<Startup>>
	{
		private readonly WebApplicationFactory<Startup> _factory;

		public MiddlewareTests(WebApplicationFactory<Startup> factory)
		{
			_factory = factory;
			_factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/v1/");
		}

		[Fact]
		public async Task Specie_Not_Found_Exception_Is_Interpreted_As_404_Error_Test()
		{
			var fakePokeService = new FakePokeApiService(true);
			var client = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton<IPokeApiService>(fakePokeService);
				});
			}).CreateClient();

			var response = await client.GetAsync($"{Helpers.BASIC_ENDPOINT}/foo");

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task PokeApi_Service_Exception_Is_Interpreted_As_502_Error_Test()
		{
			var fakePokeService = new FakePokeApiService(false, true);
			var client = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton<IPokeApiService>(fakePokeService);
				});
			}).CreateClient();

			var response = await client.GetAsync($"{Helpers.BASIC_ENDPOINT}/foo");

			Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
		}
	}
}
