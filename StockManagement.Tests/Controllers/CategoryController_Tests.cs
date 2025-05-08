using GenericStockManagement;
using GenericStockManagement.Controllers;
using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using GenericStockManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace StockManagement.Tests.Controllers
{
    public class CategoryController_Tests
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
        public void Create_Post_ValidModel_AddsCategoryAndRedirects()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
            var controller = new CategoryController(catRepository.Object, proRepository.Object, sessionServiceMock.Object);

            var category = new Category { Name = "Test", Description = "Test Desc" };

            var result = controller.Create(category);

            catRepository.Verify(r => r.Add(It.Is<Category>(c => c.Name == "Test")), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect.ActionName, Is.EqualTo("Index"));
            Assert.That(redirect.ControllerName, Is.EqualTo("Category"));
        }

        [Test]
        public void Delete_WithValidIds_ReturnsViewWithCategoryProducts()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");
            var category = new Category { Id = 1, Name = "Cat1" };
            var products = new List<Product> {
                new Product { Name = "P1", CategoryId = 1 }
            };

            catRepository.Setup(r => r.GetById(1)).Returns(category);
            proRepository.Setup(r => r.GetAll()).Returns(products);

            var controller = new CategoryController(catRepository.Object, proRepository.Object, sessionServiceMock.Object);

            var result = controller.Delete(new List<int> { 1 });

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            var model = viewResult?.Model as CategoryListViewModel;
            Assert.That(model, Is.Not.Null);
            Assert.That(model._categories.Count(), Is.EqualTo(1));
            Assert.That(model.CategoryProducts[1].Count, Is.EqualTo(1));
        }

        [Test]
        public void ConfirmDelete_DeletesEachCategoryAndRedirects()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");

            var controller = new CategoryController(catRepository.Object, proRepository.Object, sessionServiceMock.Object);

            var result = controller.ConfirmDelete(new List<int> { 1 });

            catRepository.Verify(r => r.Delete(1), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect.ActionName, Is.EqualTo("Index"));
            Assert.That(redirect.ControllerName, Is.EqualTo("Category"));
        }

        [Test]
        public void Update_WithValidIds_ReturnsCategoriesInViewModel()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");

            var category = new Category { Id = 1, Name = "Cat1" };
            catRepository.Setup(r => r.GetById(1)).Returns(category);

            var controller = new CategoryController(catRepository.Object, proRepository.Object, sessionServiceMock.Object);

            var result = controller.Update(new List<int> { 1 });

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            var model = viewResult?.Model as CategoryListViewModel;
            Assert.That(model, Is.Not.Null);
            Assert.That(model._categories.Count(), Is.EqualTo(1));
            Assert.That(model._categories.First().Name, Is.EqualTo("Cat1"));
        }

        [Test]
        public void ConfirmUpdate_UpdatesEachCategoryAndRedirects()
        {
            sessionServiceMock.Setup(s => s.GetRole()).Returns("Admin");

            var controller = new CategoryController(catRepository.Object, proRepository.Object, sessionServiceMock.Object);

            var updatedCategory = new Category { Name = "Updated", Description = "Updated Desc" };

            var result = controller.ConfirmUpdate(new List<Category> { updatedCategory });

            catRepository.Verify(r => r.Update(It.Is<Category>(c => c.Name == "Updated")), Times.Once);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect.ActionName, Is.EqualTo("Index"));
            Assert.That(redirect.ControllerName, Is.EqualTo("Category"));
        }
    }
}
