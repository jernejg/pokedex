using AutoMapper;
using MediatR;
using Pokedex.Services.FunTranslations;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex
{
	public class FunPokemonFeatureHandler : IRequestHandler<FunPokemonInfoRequest, PokemonInfoResponse>
	{
		private readonly IFunTranslationsService _funTranslationsService;
		private readonly IPokeApiService _pokeApiService;
		private readonly IMapper _mapper;

		public FunPokemonFeatureHandler(IFunTranslationsService funTranslationsService, IPokeApiService pokeApiService, IMapper mapper)
		{
			_funTranslationsService = funTranslationsService;
			_pokeApiService = pokeApiService;
			_mapper = mapper;
		}

		public async Task<PokemonInfoResponse> Handle(FunPokemonInfoRequest request, CancellationToken cancellationToken)
		{
			var pokemonSpecie = await _pokeApiService.GetValidSpecieAsync(request.Name);
			var result = _mapper.Map<PokemonInfoResponse>(pokemonSpecie);

			Func<string, Task<TranslateResponse>> target;

			if (pokemonSpecie.IsLegendary || string.Compare("cave", pokemonSpecie.Habitat.Name, StringComparison.OrdinalIgnoreCase) == 0)
			{
				target = _funTranslationsService.GetYodaTranslationsAsync;
			}
			else
			{
				target = _funTranslationsService.GetShakespeareTranslationsAsync;
			}

			try
			{
				result.Description = (await target(result.Description)).Contents.Translated;
			}
			catch (Exception e)
			{
				Log.Warning(e, "Using the default description.");
			}


			return result;
		}
	}
}
