using MediatR;

namespace Pokedex
{
	public class BasicPokemonInfoRequest : IRequest<PokemonInfoResponse>
	{
		public string Name { get; }
		public BasicPokemonInfoRequest(string name)
		{
			Name = name;
		}
	}
}
