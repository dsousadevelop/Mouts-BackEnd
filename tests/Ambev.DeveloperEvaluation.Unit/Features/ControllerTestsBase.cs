using AutoMapper;
using MediatR;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Features;

public abstract class ControllerTestsBase
{
    protected readonly IMediator Mediator;
    protected readonly IMapper Mapper;

    protected ControllerTestsBase()
    {
        Mediator = Substitute.For<IMediator>();
        Mapper = Substitute.For<IMapper>();
    }
}
