using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex
{
	public class BasicPokemonFeatureHandler : IRequestHandler<BasicPokemonInfoRequest, PokemonInfoResponse>
	{
		private readonly IPokeApiService _pokeApiService;
		private readonly IMapper _mapper;

		public BasicPokemonFeatureHandler(IPokeApiService pokeApiService, IMapper mapper)
		{
			_pokeApiService = pokeApiService;
			_mapper = mapper;
		}
		public async Task<PokemonInfoResponse> Handle(BasicPokemonInfoRequest request, CancellationToken cancellationToken)
		{
			var pokemonSpeci = await _pokeApiService.GetValidSpecieAsync(request.Name);
			return _mapper.Map<PokemonInfoResponse>(pokemonSpeci);
		}
	}
}
