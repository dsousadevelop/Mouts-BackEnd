using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CreateCategoryHandler _createCategoryHandler;
        private readonly IMapper _mapper;
        public CreateCategoryCommandHandlerTests()
        {
            _categoryRepository = Substitute.For<ICategoryRepository>();
            _mapper = Substitute.For<IMapper>();
            _createCategoryHandler = new CreateCategoryHandler(_categoryRepository, _mapper);

        }
        [Fact]
        public async Task Handle_DeveCriarCategoriaERetornarId()
        {
            var repository = Substitute.For<ICategoryRepository>();

            var entityDto = new CategoryDto(id: null, description: "Feijão");
            var command = new CreateCategoryCommand(entityDto);
            var handler = new CreateCategoryHandler(repository, _mapper);
            var categoryEntity = new Category(id: 1, description: "Feijão");

            repository.CreateAsync(Arg.Any<Category>())
                  .Returns(Task.FromResult(categoryEntity));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.AsT0.Id);
            await repository.Received(1)
                .CreateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        }
    }
}
