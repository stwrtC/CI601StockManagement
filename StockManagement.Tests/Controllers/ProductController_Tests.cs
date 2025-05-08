using GenericStockManagement.Controllers;
using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using GenericStockManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagement.Tests.Controllers
{
    public class ProductController_Tests
    {

        private static Mock<IRepository<Product>> proRepository;
        private static Mock<IRepository<Category>> catRepository;
        [SetUp]
        public void Setup()
        {
            proRepository = new Mock<IRepository<Product>>();
            catRepository = new Mock<IRepository<Category>>();
        }

        [Test]
        public void Create()
        {
            var controller = new ProductController(proRepository.Object, null);
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Brand = "Test Brand",
                Description = "Test Description",
                ImageThumbnail = "test.jpg",
                Quantity = 10,
                Price = 99.99m,
                CategoryId = 1
            };
            proRepository.Setup(x => x.Add(product)).Returns(product);
            proRepository.Setup(x => x.GetById(product.Id)).Returns(product);

            var result = controller.Create(product);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public void Delete_WithValidIds_ReturnsViewWithModel()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Brand = "Test Brand",
                Description = "Test Description",
                ImageThumbnail = "test.jpg",
                Quantity = 10,
                Price = 99.99m,
                CategoryId = 1
            };

            var category = new Category { Id = 1, Name = "Test Category" };

            proRepository.Setup(x => x.GetById(product.Id)).Returns(product);
            catRepository.Setup(x => x.GetAll()).Returns(new List<Category> { category });

            var controller = new ProductController(proRepository.Object, catRepository.Object);

            var result = controller.Delete(new List<int> { product.Id });

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.TypeOf<HomeViewModel>());
        }



        [Test]
        public void ConfirmDelete_WithValidIds_DeletesProductsAndRedirects()
        {
            var controller = new ProductController(proRepository.Object, null);

            var productId = 1;

            var result = controller.ConfirmDelete(new List<int> { productId });

            proRepository.Verify(r => r.Delete(productId), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
        }





    }
}
