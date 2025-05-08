using GenericStockManagement;
using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagement.Tests.Repository
{
    public class ProductRepository_Tests
    {
        [Test]
        public void AddProduct()
        {
            var product = new Product
            {
                Name = "Test Product",
                Brand = "Test Brand",
                Description = "Test Description",
                ImageThumbnail = "test.jpg",
                Quantity = 10,
                Price = 99.99m,
                CategoryId = 1
            };

            var options = new DbContextOptionsBuilder<StockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using (var context = new StockDbContext(options))
            {
                var repo = new ProductRepository(context);
                repo.Add(product);
                var Products = repo.GetAll();
                Assert.That(Products, Does.Contain(product));
            }
        }
        [Test]
        public void GetAll()
        {

            var options = new DbContextOptionsBuilder<StockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using (var context = new StockDbContext(options))
            {
                var repo = new ProductRepository(context);

                context.Products.Add(new Product
                {
                    Name = "Test Product",
                    Brand = "Test Brand",
                    Description = "Test Description",
                    ImageThumbnail = "test.jpg",
                    Quantity = 10,
                    Price = 99.99m,
                    CategoryId = 1
                });
                context.Products.Add(new Product 
                {
                    Name = "Test Product2",
                    Brand = "Test Brand",
                    Description = "Test Description",
                    ImageThumbnail = "test.jpg",
                    Quantity = 12,
                    Price = 9.99m,
                    CategoryId = 1
                });

                context.SaveChanges();
                var Products = repo.GetAll();

                Assert.That(Products, Is.Not.Null);
                Assert.That(Products.Count, Is.AtLeast(2));
                Assert.That(Products.Any(p => p.Name == "Test Product"), Is.True);
            }
        }

        [Test]
        public void Update()
        {
            var options = new DbContextOptionsBuilder<StockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using (var context = new StockDbContext(options))
            {
                var repo = new ProductRepository(context);
                var product = new Product
                {
                    Name = "Test Product",
                    Brand = "Test Brand",
                    Description = "Test Description",
                    ImageThumbnail = "test.jpg",
                    Quantity = 10,
                    Price = 99.99m,
                    CategoryId = 1
                };

                context.Products.Add(product);
                context.SaveChanges();

                product.Price = 249;
                product.Quantity = 7;

                var updated = repo.Update(product);

                Assert.Multiple(() =>
                {
                    Assert.That(updated.Price, Is.EqualTo(product.Price));
                    Assert.That(updated.Quantity, Is.EqualTo(product.Quantity));
                });
            }
        }

        [Test]
        public void Delete()
        {
            var options = new DbContextOptionsBuilder<StockDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using (var context = new StockDbContext(options))
            {
                var repo = new ProductRepository(context);
                var product = new Product
                {
                    Name = "Test Product",
                    Brand = "Test Brand",
                    Description = "Test Description",
                    ImageThumbnail = "test.jpg",
                    Quantity = 10,
                    Price = 99.99m,
                    CategoryId = 1
                };

                context.Products.Add(product);
                context.Products.Add(new Product
                {
                    Name = "Test Product2",
                    Brand = "Test Brand",
                    Description = "Test Description",
                    ImageThumbnail = "test.jpg",
                    Quantity = 12,
                    Price = 9.99m,
                    CategoryId = 1
                });

                context.SaveChanges();

                repo.Delete(product.Id);
                var Products = repo.GetAll();

                Assert.That(Products, Does.Not.Contain(product));
            }
        }
    }
}
