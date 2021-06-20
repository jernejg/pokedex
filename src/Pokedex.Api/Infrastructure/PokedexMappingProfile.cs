namespace Pokedex.Profile
{
	public class PokedexMappingProfile : AutoMapper.Profile
	{
		public PokedexMappingProfile()
		{
			CreateMap<PokemonSpecieResponse, PokemonInfoResponse>()
				.ForMember(dest => dest.Habitat, opt => opt.MapFrom(src => src.Habitat.Name))
				.ForMember(dest => dest.Description, opt =>
					opt.MapFrom(src =>
						src.GetFirstEnglishFlavorTextEntry().FlavorText
				));
		}
	}
}
