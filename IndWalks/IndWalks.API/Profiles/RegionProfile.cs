using AutoMapper;

namespace IndWalks.API.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();

            //if columns are different we need to map like
            // .ForMember(des => des.Id, options => options.MapFrom(src => src.Id));

        }
    }
}
