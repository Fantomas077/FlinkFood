using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using FlinkFood.Services;
using FlinkFood.Services.Extensions;
using Microsoft.Identity.Client;
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
        [Test]
        public void AddCategoryAsync_WhenCategoryAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var category = new Category { Id = 2, Name = "Pizza" };

            _repoMock
                .Setup(r => r.ExistsByNameAsync("pizza"))
                .ReturnsAsync(true);

            // Act + Assert
            Assert.ThrowsAsync<CategoryAlreadyExisted>(() =>
                _service.AddCategoryAsync(category)
            );

            // 
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Never);
        }
        [Test]
        public async Task UpdateCategoryAsync_WhenDataIsValid_ShouldCallRepository()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "  PIZZA  " };

            
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(new Category { Id = 1, Name = "pizza" });

            
            _repoMock
                .Setup(r => r.ExistsByNameAsync("pizza", 1))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateCategoryAsync(category);

            // Assert
            _repoMock.Verify(
                r => r.UpdateAsync(It.Is<Category>(c => c.Name == "pizza")),
                Times.Once
            );
        }
        [Test]
        public void UpdateCategoryAsync_NotFoundId_ShouldThrowException()
        {
            // Arrange
            var category = new Category { Id = 2, Name = "Pizza" };

            
            _repoMock
                .Setup(r => r.FindById(2))
                .ReturnsAsync((Category?)null);

            // Act + Assert
            Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpdateCategoryAsync(category)
            );

            // 
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }
        [Test]
        public void UpdateCategoryAsync_WhenNameAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var category = new Category { Id = 2, Name = "Pizza" };

            // Arrange
            _repoMock
                .Setup(r => r.FindById(2))
                .ReturnsAsync(new Category { Id = 2, Name = "pizza" });

            
            _repoMock
                .Setup(r => r.ExistsByNameAsync("pizza", 2))
                .ReturnsAsync(true);

            // Act + Assert
            Assert.ThrowsAsync<CategoryAlreadyExisted>(() =>
                _service.UpdateCategoryAsync(category)
            );

            // 
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }
        [Test]
        public async Task GetById_WhenIdValid_ShouldReturnCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Pizza" };

            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(category);

            // Act
            var result = await _service.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Pizza"));

            _repoMock.Verify(r => r.FindById(1), Times.Once);
        }
        [Test]
        public async Task GetById_WhenIdNotFound_ShouldThrowException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.FindById(99))
                .ReturnsAsync((Category?)null);

            // Act + Assert
            Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetById(99)
            );

            _repoMock.Verify(r => r.FindById(99), Times.Once);
        }
        [Test]
        public async Task DeleteCategoryAsync_WhenDataValid_ShouldDeleteCategory()
        {
            var category = new Category { Id = 1, Name = "Pizza" };
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(category);

            _repoMock
                .Setup(r => r.DeleteAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCategoryAsync(category);

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(It.Is<Category>(c => c.Name == "Pizza")), Times.Once);
        }
        [Test]
        public async Task DeleteCategoryAsync_NotFoundId_ShouldThrowNewException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.FindById(99))
                .ReturnsAsync((Category?)null);

            // Act + Assert
            Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteCategoryAsync(new Category { Id = 99 }));

            // Assert
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
        }
        [Test]
        public async Task GetAll_WhenCategoriesExist_ShouldReturnListOfCategories()
        {
            // Arrange
            var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Pizza" },
        new Category { Id = 2, Name = "Burger" }
    };

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Pizza"));
            Assert.That(result.Any(c => c.Name == "Burger"));

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
        [Test]
        public async Task GetAll_WhenNoCategoriesExist_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<Category>();

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
            Assert.That(result, Is.Empty);

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

    }
}
