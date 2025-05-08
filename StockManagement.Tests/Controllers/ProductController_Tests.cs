using GenericStockManagement;
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

        private static Mock<IRepository<Category>> catRepository;
        private static Mock<IRepository<Product>> proRepository;
        private static Mock<ISessionService> sessionServiceMock;

        [SetUp]
        public void Setup()
        {
            catRepository = new Mock<IRepository<Category>>();
            proRepository = new Mock<IRepository<Product>>();
            sessionServiceMock = new Mock<ISessionService>();
        }

        [Test]
        public void Create()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
            var controller = new ProductController(proRepository.Object, null, sessionServiceMock.Object);
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
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
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

            var controller = new ProductController(proRepository.Object, catRepository.Object, sessionServiceMock.Object);

            var result = controller.Delete(new List<int> { product.Id });

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.TypeOf<HomeViewModel>());
        }



        [Test]
        public void ConfirmDelete_WithValidIds_DeletesProductsAndRedirects()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
            var controller = new ProductController(proRepository.Object, null, sessionServiceMock.Object);

            var productId = 1;

            var result = controller.ConfirmDelete(new List<int> { productId });

            proRepository.Verify(r => r.Delete(productId), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Product"));
        }

        [Test]
        public void Update_WithValidIds_ReturnsViewWithProductsAndCategories()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Brand = "Brand",
                Description = "Description",
                ImageThumbnail = "image.jpg",
                Quantity = 5,
                Price = 100,
                CategoryId = 2
            };

            var category = new Category { Id = 2, Name = "Category 2" };

            proRepository.Setup(r => r.GetById(productId)).Returns(product);
            catRepository.Setup(r => r.GetAll()).Returns(new List<Category> { category });

            var controller = new ProductController(proRepository.Object, catRepository.Object, sessionServiceMock.Object);

            var result = controller.Update(new List<int> { productId });

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            var model = viewResult?.Model as ProductUpdateViewModel;
            Assert.IsNotNull(model);
            Assert.That(model.Products.Count, Is.EqualTo(1));
            Assert.That(model.Categories.Count(), Is.EqualTo(1));
            Assert.That(model.Products[0].Name, Is.EqualTo("Updated Product"));
        }

        [Test]
        public void ConfirmUpdate_ValidProducts_UpdatesRepositoryAndRedirects()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
            var productId = 1;
            var originalProduct = new Product
            {
                Id = productId,
                Name = "Old Name",
                CategoryId = 1,
                Brand = "Old Brand",
                Description = "Old Desc",
                Price = 10,
                Quantity = 1,
                ImageThumbnail = "old.jpg"
            };

            var updatedProduct = new ProductEditViewModel
            {
                Id = productId,
                Name = "New Name",
                CategoryId = 2,
                Brand = "New Brand",
                Description = "New Desc",
                Price = 99.99m,
                Quantity = 10,
                ImageThumbnail = "new.jpg"
            };

            proRepository.Setup(r => r.GetById(productId)).Returns(originalProduct);

            var controller = new ProductController(proRepository.Object, catRepository.Object, sessionServiceMock.Object);

            var result = controller.ConfirmUpdate(new List<ProductEditViewModel> { updatedProduct });

            proRepository.Verify(r => r.Update(It.Is<Product>(p =>
                p.Id == productId &&
                p.Name == "New Name" &&
                p.CategoryId == 2 &&
                p.Brand == "New Brand" &&
                p.Description == "New Desc" &&
                p.Price == 99.99m &&
                p.Quantity == 10 &&
                p.ImageThumbnail == "new.jpg"
            )), Times.Once);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect?.ActionName, Is.EqualTo("Index"));
            Assert.That(redirect?.ControllerName, Is.EqualTo("Product"));
        }

    }
}
