using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex
{
	[Route("v1/[controller]")]
	[ApiController]
	public class PokemonController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PokemonController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{pokemonName}")]
		public async Task<IActionResult> GetBasicPokemonInfo(string pokemonName, CancellationToken cancellationToken)
		{
			var basicPokemonInfo = await _mediator.Send(new BasicPokemonInfoRequest(pokemonName), cancellationToken);
			return Ok(basicPokemonInfo);
		}

		[HttpGet("translated/{pokemonName}")]
		public async Task<IActionResult> GetFunPokemonInfo(string pokemonName, CancellationToken cancellationToken)
		{
			var funPokemonInfo = await _mediator.Send(new FunPokemonInfoRequest(pokemonName), cancellationToken);
			return Ok(funPokemonInfo);
		}
	}
}
