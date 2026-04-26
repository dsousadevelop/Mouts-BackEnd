using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Common
{
    public class PaginatedListTests
    {
        [Fact(DisplayName = "O construtor deve inicializar as propriedades corretamente")]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var items = new List<int> { 1, 2, 3 };
            const int count = 10;
            const int pageNumber = 1;
            const int pageSize = 3;

            // Act
            var paginatedList = new PaginatedList<int>(items, count, pageNumber, pageSize);

            // Assert
            paginatedList.Should().HaveCount(3);
            paginatedList.TotalCount.Should().Be(count);
            paginatedList.CurrentPage.Should().Be(pageNumber);
            paginatedList.PageSize.Should().Be(pageSize);
            paginatedList.TotalPages.Should().Be(4); // 10 / 3 = 3.33 -> 4
        }

        [Theory(DisplayName = "HasPrevious e HasNext devem retornar os valores corretos conforme a pÃ¡gina")]
        [InlineData(1, 10, 3, false, true)]  // PÃ¡gina 1 de 4: NÃ£o tem anterior, tem prÃ³xima
        [InlineData(2, 10, 3, true, true)]   // PÃ¡gina 2 de 4: Tem anterior, tem prÃ³xima
        [InlineData(4, 10, 3, true, false)]  // PÃ¡gina 4 de 4: Tem anterior, nÃ£o tem prÃ³xima
        [InlineData(1, 3, 3, false, false)]  // PÃ¡gina 1 de 1: NÃ£o tem anterior, nÃ£o tem prÃ³xima
        public void HasPreviousAndHasNext_ShouldReturnCorrectValues(int pageNumber, int count, int pageSize, bool expectedHasPrevious, bool expectedHasNext)
        {
            // Arrange
            var items = new List<int>();

            // Act
            var paginatedList = new PaginatedList<int>(items, count, pageNumber, pageSize);

            // Assert
            paginatedList.HasPrevious.Should().Be(expectedHasPrevious);
            paginatedList.HasNext.Should().Be(expectedHasNext);
        }

        [Fact(DisplayName = "TotalPages deve ser 0 quando o total de itens for 0")]
        public void TotalPages_ShouldBeZero_WhenCountIsZero()
        {
            // Arrange
            var items = new List<int>();
            const int count = 0;
            const int pageNumber = 1;
            const int pageSize = 10;

            // Act
            var paginatedList = new PaginatedList<int>(items, count, pageNumber, pageSize);

            // Assert
            paginatedList.TotalPages.Should().Be(0);
        }

        [Fact(DisplayName = "AddRange deve adicionar os itens Ã  lista base")]
        public void AddRange_ShouldAddItemsToList()
        {
            // Arrange
            var items = new List<int> { 10, 20 };
            const int count = 2;
            const int pageNumber = 1;
            const int pageSize = 10;

            // Act
            var paginatedList = new PaginatedList<int>(items, count, pageNumber, pageSize);

            // Assert
            paginatedList.Should().ContainInOrder(10, 20);
        }
    }
}
