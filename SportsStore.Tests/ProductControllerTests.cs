using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests {

    public class ProductControllerTests {

        [Fact]
        public void Can_Paginate() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductsListViewModel result =
                (controller.List(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }


        [Fact]
        public void Can_Send_Pagination_View_Model() {

            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());

            // Arrange
            ProductController controller =
                new ProductController(mock.Object) { PageSize = 3 };

            // Act
            ProductsListViewModel result =
                (controller.List(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
        [Fact]
        public void CanFilterProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category="Cat"},
                new Product {ProductID = 2, Name = "P2", Category="Cat"},
                new Product {ProductID = 3, Name = "P3", Category="Cat"},
                new Product {ProductID = 4, Name = "P4", Category="Dog"},
                new Product {ProductID = 5, Name = "P5", Category="Cat"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 4;

            ProductsListViewModel result =
                (controller.List("Cat", 1) as ViewResult).ViewData.Model as ProductsListViewModel;

            Product[] products = result.Products.ToArray();

            Assert.True(products[0].Name == "P1" && products[0].Category == "Cat");
            Assert.Equal("P5", products[3].Name);
            Assert.Equal("Cat", products[3].Category);
        }

        [Fact]
        public void CanCountProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category="Cat"},
                new Product {ProductID = 2, Name = "P2", Category="Human"},
                new Product {ProductID = 3, Name = "P3", Category="Cat"},
                new Product {ProductID = 4, Name = "P4", Category="Dog"},
                new Product {ProductID = 5, Name = "P5", Category="Dog"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);

            int? res1 = GetModel (controller.List("Cat"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(controller.List("Human"))?.PagingInfo.TotalItems;
            int? res3= GetModel(controller.List("Dog"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(controller.List(null))?.PagingInfo.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(1, res2);
            Assert.Equal(2, res3);
            Assert.Equal(5, resAll);
        }

        private ProductsListViewModel GetModel(IActionResult viewResult) => 
            (viewResult as ViewResult).ViewData.Model as ProductsListViewModel;

    }
}
