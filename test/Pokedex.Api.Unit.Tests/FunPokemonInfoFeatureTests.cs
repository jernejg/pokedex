using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Unit.Tests
{
	public class FunPokemonInfoFeatureTests
	{
		[Fact]
		public async Task Happy_Path_Test()
		{

		}
		[Fact]
		public async Task Only_Yoda_Translator_Is_Used_For_Cave_Pokemons_Test()
		{

		}

		[Fact]
		public async Task Only_Yoda_Translator_Is_Used_For_Legendary_Pokemons_Test()
		{

		}
		[Fact]
		public async Task Only_Yoda_Translator_Is_Used_For_Legendary_And_Cave_Pokemons_Test()
		{

		}
		[Fact]
		public async Task Only_Shakespeare_Translator_Is_Used_For_Non_Cave_And_Non_Legendary_Pokemons_Test()
		{

		}

		[Fact]
		public async Task Use_Original_Pokemon_Description_If_Translation_Can_Not_Be_Retrieved_Test()
		{

		}

		[Fact]
		public async Task Translation_Service_Exception_Is_Raised_When_Translation_Cant_Be_Retrieved_Test()
		{

		}

		[Fact]
		public async Task Translation_Service_Exception_Is_Raised_When_Translation_Response_Does_Not_Indicate_Success_Test()
		{

		}
	}
}