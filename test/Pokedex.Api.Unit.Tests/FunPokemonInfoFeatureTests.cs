using AutoMapper;
using Moq;
using Pokedex.Profile;
using Pokedex.Services.FunTranslations;
using Pokedex.Unit.Tests.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Unit.Tests
{
	public class FunPokemonInfoFeatureTests
	{
		[Fact]
		public async Task Happy_Path_Test()
		{
			//Arrange
			var pokemon = PokemonBuilder.BuildLegendaryCavePokemon();
			var translateResponse = TranslationBuilder.BuildTranslation();
			//Act
			var r = await GetSut(pokemon).Handle(new FunPokemonInfoRequest(pokemon.Name), CancellationToken.None);
			//Assert
			Assert.Equal(translateResponse.Contents.Translated, r.Description);
		}
		[Fact]
		public async Task Only_Yoda_Translator_Is_Used_For_Cave_Pokemons_Test()
		{
			//Arrange
			var mock = new Mock<IFunTranslationsService>();
			var pokemon = PokemonBuilder.BuildNonLegendaryCavePokemon();
			//Act
			await GetSut(pokemon, mock).Handle(new FunPokemonInfoRequest(pokemon.Name), CancellationToken.None);
			//Assert
			mock.Verify(m => m.GetYodaTranslationsAsync(pokemon.GetFirstEnglishFlavorTextEntry().FlavorText), Times.Once);
			mock.Verify(m => m.GetShakespeareTranslationsAsync(It.IsAny<string>()), Times.Never());
		}

		[Fact]
		public async Task Only_Yoda_Translator_Is_Used_For_Legendary_Pokemons_Test()
		{
			//Arrange
			var mock = new Mock<IFunTranslationsService>();
			var pokemon = PokemonBuilder.BuildLegendaryNonCavePokemon();
			//Act
			await GetSut(pokemon, mock).Handle(new FunPokemonInfoRequest(pokemon.Name), CancellationToken.None);

			mock.Verify(m => m.GetYodaTranslationsAsync(pokemon.GetFirstEnglishFlavorTextEntry().FlavorText), Times.Once);
			mock.Verify(m => m.GetShakespeareTranslationsAsync(It.IsAny<string>()), Times.Never());
		}
		[Fact]
		public async Task Only_Yoda_Translator_Is_Used_For_Legendary_And_Cave_Pokemons_Test()
		{
			//Arrange
			var mock = new Mock<IFunTranslationsService>();
			var pokemon = PokemonBuilder.BuildLegendaryCavePokemon();
			//Act
			await GetSut(pokemon, mock).Handle(new FunPokemonInfoRequest(pokemon.Name), CancellationToken.None);

			mock.Verify(m => m.GetYodaTranslationsAsync(pokemon.GetFirstEnglishFlavorTextEntry().FlavorText), Times.Once);
			mock.Verify(m => m.GetShakespeareTranslationsAsync(It.IsAny<string>()), Times.Never());
		}
		[Fact]
		public async Task Only_Shakespeare_Translator_Is_Used_For_Non_Cave_And_Non_Legendary_Pokemons_Test()
		{
			//Arrange
			var mock = new Mock<IFunTranslationsService>();
			var pokemon = PokemonBuilder.BuildNonLegendaryNonCavePokemon();
			//Act
			await GetSut(pokemon, mock).Handle(new FunPokemonInfoRequest(pokemon.Name), CancellationToken.None);
			//Assert
			mock.Verify(m => m.GetShakespeareTranslationsAsync(pokemon.GetFirstEnglishFlavorTextEntry().FlavorText), Times.Once);
			mock.Verify(m => m.GetYodaTranslationsAsync(It.IsAny<string>()), Times.Never());
		}

		[Fact]
		public async Task Use_Original_Pokemon_Description_If_Translation_Can_Not_Be_Retrieved_Test()
		{
			//Arrange
			var mock = new Mock<IFunTranslationsService>();
			var pokemon = PokemonBuilder.BuildLegendaryCavePokemon();
			mock.Setup(m => m.GetYodaTranslationsAsync(It.IsAny<string>())).Throws<Exception>();
			//Act
			var r = await GetSut(pokemon, mock).Handle(new FunPokemonInfoRequest(pokemon.Name), CancellationToken.None);
			Assert.Equal(pokemon.GetFirstEnglishFlavorTextEntry().FlavorText, r.Description);
		}

		[Fact]
		public async Task Translation_Service_Exception_Is_Raised_When_Translation_Cant_Be_Retrieved_Test()
		{
			//Arrange
			var httpClient = Helpers.GetHttpClient(new StringContent(JsonSerializer.Serialize(TranslationBuilder.BuildTranslation())), HttpStatusCode.ServiceUnavailable);
			var sut = new FunTranslationsRestApiService(httpClient);
			//Act & Assert
			var ex = await Assert.ThrowsAsync<TranslationServiceException>(() => sut.GetShakespeareTranslationsAsync("foo"));
			Assert.Equal("Pokedex is experiencing problems with Translation service provider", ex.Message);
		}

		[Fact]
		public async Task Translation_Service_Exception_Is_Raised_When_Translation_Response_Does_Not_Indicate_Success_Test()
		{
			//Arrange
			var translationResponse = TranslationBuilder.BuildTranslation();
			translationResponse.Success.Total = 0;
			var httpClient = Helpers.GetHttpClient(new StringContent(JsonSerializer.Serialize(translationResponse)), HttpStatusCode.ServiceUnavailable);
			var sut = new FunTranslationsRestApiService(httpClient);
			//Act & Assert
			var ex = await Assert.ThrowsAsync<TranslationServiceException>(() => sut.GetShakespeareTranslationsAsync("foo"));
			Assert.Equal("Pokedex is experiencing problems with Translation service provider", ex.Message);
		}

		private static FunPokemonFeatureHandler GetSut(PokemonSpecieResponse pokemonModel, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			return GetSut(pokemonModel, TranslationBuilder.BuildTranslation(), statusCode);
		}

		private static FunPokemonFeatureHandler GetSut(PokemonSpecieResponse pokemonModel, TranslateResponse translateResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var mockPokeService = new Mock<IPokeApiService>();
			mockPokeService.Setup(m => m.GetValidSpecieAsync(It.IsAny<string>()).Result).Returns(pokemonModel);
			var x = Helpers.GetHttpClient(new StringContent(JsonSerializer.Serialize(translateResponse)), statusCode);
			var translationService = new FunTranslationsRestApiService(x);
			var mockMapper = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new PokedexMappingProfile());
			});
			var sut = new FunPokemonFeatureHandler(translationService, mockPokeService.Object, mockMapper.CreateMapper());

			return sut;
		}

		private static FunPokemonFeatureHandler GetSut(PokemonSpecieResponse pokemonModel, Mock<IFunTranslationsService> mockTranslationService)
		{
			var mockPokeService = new Mock<IPokeApiService>();
			mockPokeService.Setup(m => m.GetValidSpecieAsync(It.IsAny<string>()).Result).Returns(pokemonModel);
			var mockMapper = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new PokedexMappingProfile());
			});
			var sut = new FunPokemonFeatureHandler(mockTranslationService.Object, mockPokeService.Object, mockMapper.CreateMapper());

			return sut;
		}
	}
}
