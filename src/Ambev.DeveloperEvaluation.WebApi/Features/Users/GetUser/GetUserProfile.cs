using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Profile for mapping GetUser feature requests to commands
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser feature
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<int, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));

        CreateMap<GetUserResult, GetUserResponse>();
        CreateMap<GetUserResult.GetUserResultName, GetUserResponse.GetUserResponseName>();
        CreateMap<GetUserResult.GetUserResultAddress, GetUserResponse.GetUserResponseAddress>();
        CreateMap<GetUserResult.GetUserResultGeolocation, GetUserResponse.GetUserResponseGeolocation>();
    }
}
