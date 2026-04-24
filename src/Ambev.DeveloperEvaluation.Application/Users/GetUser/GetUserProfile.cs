using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Profile for mapping between User entity and GetUserResponse
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser operation
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<User, GetUserResult>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

        CreateMap<User, GetUserResult.GetUserResultName>()
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.LastName));

        CreateMap<Address, GetUserResult.GetUserResultAddress>()
            .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.ZipCode))
            .ForMember(dest => dest.Geolocation, opt => opt.MapFrom(src => src));

        CreateMap<Address, GetUserResult.GetUserResultGeolocation>()
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Geolocation_lat))
            .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Geolocation_long));
    }
}
