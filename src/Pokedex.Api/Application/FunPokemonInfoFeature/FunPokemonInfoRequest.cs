using MediatR;

namespace Pokedex
{
	public class FunPokemonInfoRequest : IRequest<PokemonInfoResponse>
	{
		public string Name { get; }
		public FunPokemonInfoRequest(string name)
		{
			Name = name;
		}
	}
}