using AutoMapper;
using Pokedex.Profile;
using Pokedex.Services.PokeApi;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Unit.Tests
{
	public class BasicPokemonInfoFeatureTests
	{
		[Fact]
		public async Task Happy_Path_Test()
		{
			//Arrange
			var returnedPokemon = PokemonBuilder.BuildLegendaryCavePokemon();
			var sut = GetSut(returnedPokemon);
			//Act
			var result = await sut.Handle(new BasicPokemonInfoRequest(returnedPokemon.Name), CancellationToken.None);
			//Assert	
			Assert.Equal(returnedPokemon.Name, result.Name);
			Assert.Equal(returnedPokemon.IsLegendary, result.IsLegendary);
			Assert.Equal(returnedPokemon.FlavorTextEntries[0].FlavorText, result.Description);
			Assert.Equal(returnedPokemon.Habitat.Name, result.Habitat);
		}

		[Fact]
		public async Task Specie_Not_Found_Exception_Is_Raised_When_Pokemon_Doesnt_Exist_Test()
		{
			await Assert.ThrowsAsync<SpecieNotFoundException>(() => GetSut(HttpStatusCode.NotFound).Handle(new BasicPokemonInfoRequest("arnold"), CancellationToken.None));
		}

		[Fact]
		public async Task Pokemon_Service_Exception_Is_Raised_When_Info_Cant_Be_Retrieved_Test()
		{
			//Arrange
			var returnedPokemon = PokemonBuilder.BuildLegendaryCavePokemon();
			//Act & Assert
			var ex = await Assert.ThrowsAsync<PokemonServiceException>(() => GetSut(returnedPokemon, HttpStatusCode.ServiceUnavailable).Handle(new BasicPokemonInfoRequest(returnedPokemon.Name), CancellationToken.None));
			Assert.Equal("Pokedex is experiencing problems with Pokemon data provider", ex.Message);
		}

		[Fact]
		public async Task Pokemon_Service_Exception_Is_Raised_If_Specie_Is_Missing_English_Description_Test()
		{
			//Arrange
			var returnedPokemon = PokemonBuilder.BuildNoEnglishDescriptionPokemon();
			//Act & Assert
			var ex = await Assert.ThrowsAsync<PokemonServiceException>(() => GetSut(returnedPokemon).Handle(new BasicPokemonInfoRequest(returnedPokemon.Name), CancellationToken.None));
			Assert.Equal($"Pokemon '{returnedPokemon.Name}' is missing english description.", ex.Message);
		}

		[Fact]
		public async Task Pokemon_Service_Exception_Is_Raised_If_Specie_Is_Missing_English_Description2_Test()
		{
			//Arrange
			var returnedPokemon = PokemonBuilder.BuildNullFlavorTextEntriesPokemon();
			//Act & Assert
			var ex = await Assert.ThrowsAsync<PokemonServiceException>(() => GetSut(returnedPokemon).Handle(new BasicPokemonInfoRequest(returnedPokemon.Name), CancellationToken.None));
			Assert.Equal($"Pokemon '{returnedPokemon.Name}' is missing english description.", ex.Message);
		}

		[Fact]
		public async Task Pokemon_Service_Exception_Is_Raised_If_Specie_Is_Missing_Habitat_Information_Test()
		{
			//Arrange
			var returnedPokemon = PokemonBuilder.BuildNullHabitatPokemon();
			//Act & Assert
			var ex = await Assert.ThrowsAsync<PokemonServiceException>(() => GetSut(returnedPokemon).Handle(new BasicPokemonInfoRequest(returnedPokemon.Name), CancellationToken.None));
			Assert.Equal($"Pokemon '{returnedPokemon.Name}' is missing habitat information.", ex.Message);
		}

		[Fact]
		public async Task Pokemon_Service_Exception_Is_Raised_If_Specie_Is_Missing_Habitat_Information2_Test()
		{
			//Arrange
			var returnedPokemon = PokemonBuilder.BuildNullHabitatNamePokemon();
			//Act & Assert
			var ex = await Assert.ThrowsAsync<PokemonServiceException>(() => GetSut(returnedPokemon).Handle(new BasicPokemonInfoRequest(returnedPokemon.Name), CancellationToken.None));
			Assert.Equal($"Pokemon '{returnedPokemon.Name}' is missing habitat information.", ex.Message);
		}

		private static BasicPokemonFeatureHandler GetSut(PokemonSpecieResponse pokeApiResponseModel, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			return GetSut(JsonSerializer.Serialize(pokeApiResponseModel), statusCode);
		}

		private static BasicPokemonFeatureHandler GetSut(HttpStatusCode statusCode)
		{
			return GetSut(String.Empty, statusCode);
		}

		private static BasicPokemonFeatureHandler GetSut(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var x = Helpers.GetHttpClient(new StringContent(json), statusCode);
			var service = new PokeApiRestService(x);
			var mockMapper = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new PokedexMappingProfile());
			});
			var mapper = mockMapper.CreateMapper();
			var sut = new BasicPokemonFeatureHandler(service, mapper);
			return sut;
		}
	}
}