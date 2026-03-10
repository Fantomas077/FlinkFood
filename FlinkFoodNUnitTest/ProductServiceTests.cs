using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using FlinkFood.Services;
using FlinkFood.Services.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlinkFoodNUnitTest
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _repoMock;
        private ProductService _service;


        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _service = new ProductService(_repoMock.Object);

        }
        [Test]
        public async Task GetAllProduct_ShouldReturnList()
        {
            //Arrange
            var productlist = new List<Product>()
            {
               new Product{Id=1,Name="Pizza"},
               new Product{Id=2,Name="Fanta"}

            };

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(productlist);
            //Act
            var result = await _service.GetAll();
            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.Not.Null);

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);


        }
        [Test]
        public async Task GetAllProduct_WithEmptyList_ShouldReturnEmptyList()
        {
            //Arrange
            var emptylist = new List<Product>();


            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptylist);
            //Act
            var result = await _service.GetAll();
            //Assert
            Assert.That(result.Count, Is.EqualTo(0));
            Assert.That(result, Is.Not.Null);

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);


        }
        [Test]
        public async Task AddProductAsync_WhenValid_ShouldAddProduct()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Pizza" };

            _repoMock
                .Setup(r => r.ExistsByNameAsync(product.Name))
                .ReturnsAsync(false);
            _repoMock
                .Setup(r => r.AddAsync(product))
                .Returns(Task.CompletedTask);
            //Act
            await _service.AddProductAsync(product);

            _repoMock.Verify(r => r.AddAsync(product), Times.Once);
        }

        [Test]
        public async Task AddProductAsync_WhenProductAlreadyExisted_ShouldThrowNewException()
        {
            // Arrange
            var product = new Product { Name = "Pizza" };

            _repoMock
                .Setup(r => r.ExistsByNameAsync("pizza"))
                .ReturnsAsync(true);

            // Act + Assert
            Assert.ThrowsAsync<ProductAlreadyExisted>(() => _service.AddProductAsync(product));

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
        }
        [Test]
        public async Task UpdateProductAsync_WhenValid_ShouldUpdateProduct()
        {
            // Arrange
            var existing = new Product { Id = 1, Name = "old" };
            var updated = new Product { Id = 1, Name = "Pizza" };

            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(existing);

            _repoMock
                .Setup(r => r.ExistsByNameAsync("pizza", 1))
                .ReturnsAsync(false);

            // Act
            await _service.UpdateProductAsync(updated);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            Assert.That(existing.Name, Is.EqualTo("pizza"));
        }
        [Test]
        public async Task UpdateProductAsync_NotFoundId_ShouldThrowNewException()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Pizza" };
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync((Product?)null);
            //Act +Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateProductAsync(product));

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }
        [Test]
        public async Task UpdateProductAsync_ProductAlreadyExisted_ShouldThrowNewException()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Pizza" };
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(product);
            _repoMock
                .Setup(r => r.ExistsByNameAsync("pizza",1))
                .ReturnsAsync(true);
            //Act +Assert
            Assert.ThrowsAsync<ProductAlreadyExisted>(()=>_service.UpdateProductAsync(product));
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }
        [Test]
        public  async Task GetById_WhenValid_ShoudReturnProduct()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Pizza" };
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(product);
            //Act+assert
            var result = await _service.GetById(product.Id);
            Assert.That(result.Name, Is.EqualTo("Pizza"));
            Assert.That(result, Is.Not.Null);
            _repoMock.Verify(r => r.FindById(1), Times.Once);

        }
        [Test]
        public async Task GetById_NotFoundId_ShouldThrowNewException()
        {
            //Arrange
            _repoMock
                .Setup(r => r.FindById(99))
                .ReturnsAsync((Product?)null);
            //Act+Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.GetById(99));
            _repoMock.Verify(r => r.FindById(99), Times.Once);

        }
        [Test]
        public async Task DeleteProductAsync_WhenValid_ShouldDeleteProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Pizza" };
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync(product);
            _repoMock
                .Setup(r => r.DeleteAsync(product))
                .Returns(Task.CompletedTask);
            //Act+Assert
            await _service.DeleteProductAsync(product);
            _repoMock.Verify(r => r.DeleteAsync(product), Times.Once);
        }
        [Test]
        public async Task DeleteProductAsync_NotFoundId_ShouldThrowNewException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Pizza" };
            _repoMock
                .Setup(r => r.FindById(1))
                .ReturnsAsync((Product?)null);

            //Act+Assert
            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteProductAsync(product));
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
        }


    }
}
