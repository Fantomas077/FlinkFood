using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using FlinkFood.Services;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FlinkFoodNUnitTest
{
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _repoMock;
        private CategoryService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repoMock.Object);
        }

        [Test]
        public async Task AddCategoryAsync_WhenDataIsValid_ShouldCallRepository()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Pizza" };

            _repoMock
                .Setup(r => r.ExistsByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddCategoryAsync(category);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == "pizza")), Times.Once);
        }

    }
}
