using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public CreateUserProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<CreateUserRequest.CreateUserRequestName, CreateUserCommand.CreateUserNameCommand>();
        CreateMap<CreateUserRequest.CreateUserRequestAddress, CreateUserCommand.CreateUserAddressCommand>();
        CreateMap<CreateUserRequest.CreateUserRequestGeolocation, CreateUserCommand.CreateUserGeolocationCommand>();
        CreateMap<CreateUserResult, CreateUserResponse>();
        CreateMap<CreateUserResult.CreateUserResultName, CreateUserResponse.CreateUserResponseName>();
        CreateMap<CreateUserResult.CreateUserResultAddress, CreateUserResponse.CreateUserResponseAddress>();
        CreateMap<CreateUserResult.CreateUserResultGeolocation, CreateUserResponse.CreateUserResponseGeolocation>();
    }
}
