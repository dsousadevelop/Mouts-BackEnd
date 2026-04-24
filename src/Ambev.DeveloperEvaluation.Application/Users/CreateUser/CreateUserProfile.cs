using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Profile for mapping between User entity and CreateUserResponse
/// </summary>
public class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public CreateUserProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.Firstname))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.Lastname));

        CreateMap<CreateUserCommand.CreateUserAddressCommand, Address>()
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Zipcode))
            .ForMember(dest => dest.Geolocation_lat, opt => opt.MapFrom(src => src.Geolocation.Lat))
            .ForMember(dest => dest.Geolocation_long, opt => opt.MapFrom(src => src.Geolocation.Long));

        CreateMap<User, CreateUserResult>()
            .ForPath(dest => dest.Name.Firstname, opt => opt.MapFrom(src => src.FirstName))
            .ForPath(dest => dest.Name.Lastname, opt => opt.MapFrom(src => src.LastName));

        CreateMap<Address, CreateUserResult.CreateUserResultAddress>()
            .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.ZipCode))
            .ForPath(dest => dest.Geolocation.Lat, opt => opt.MapFrom(src => src.Geolocation_lat))
            .ForPath(dest => dest.Geolocation.Long, opt => opt.MapFrom(src => src.Geolocation_long));
    }
}
