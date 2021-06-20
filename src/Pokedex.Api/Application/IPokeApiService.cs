using System.Threading.Tasks;

namespace Pokedex
{
	public interface IPokeApiService
	{
		Task<PokemonSpecieResponse> GetValidSpecieAsync(string name);
	}
}
