using SportsStore.Models;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void IndexContainsAllProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product{Name = "P1"},
                    new Product{Name = "P2"},
                    new Product{Name="P3"},
                    new Product{Name ="P4"}
                }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product[] productResult = GiveViewModel<IEnumerable<Product>>(target.Index()).ToArray();

            Assert.Equal(4, productResult.Count());

            Assert.Equal("P1", productResult[0].Name);
            Assert.Equal("P2", productResult[1].Name);
            Assert.Equal("P3", productResult[2].Name);
            Assert.Equal("P4", productResult[3].Name);
        }

        private T GiveViewModel<T>(IActionResult result) where T : class => (result as ViewResult).ViewData?.Model as T;

        [Fact]
        public void CanEditProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
            new Product{Name ="P1", ProductID= 1},
            new Product{Name = "P2", ProductID = 2},
            new Product{Name = "P3", ProductID =3}
        }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product result1 = GiveViewModel<Product>(target.Edit(1));
            Product result2 = GiveViewModel<Product>(target.Edit(2));
            Product result3 = GiveViewModel<Product>(target.Edit(3));

            Assert.Equal(1, result1.ProductID);
            Assert.Equal(2, result2.ProductID);
            Assert.Equal(3, result3.ProductID);
        }


        [Fact]
        public void CanEditNonexistentProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
            new Product{Name ="P1", ProductID= 1},
            new Product{Name = "P2", ProductID = 2},
            new Product{Name = "P3", ProductID =3}
        }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product result = GiveViewModel<Product>(target.Edit(4));

            Assert.Null(result);
        }

        [Fact]
        public void CanSaveValidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Product product = new Product { Name = "Test" };

            IActionResult result = target.Edit(product);

            mock.Verify(m => m.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }
        //BindNever, ModelState, Attch, testing 

        [Fact]
        public void CannotSaveINvalidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");

            IActionResult result = target.Edit(product);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);

            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void CanDeleteProduct()
        {
            Product productToDelete = new Product { Name = "P2", ProductID = 2 };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{Name ="P1",ProductID = 1},
                productToDelete,
                new Product{Name ="P3", ProductID =3}
            }.AsQueryable());

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            target.Delete(productToDelete.ProductID);

            mock.Verify(m => m.DeleteProduct(productToDelete.ProductID));
        }
    }
}
