using Microsoft.AspNetCore.Mvc.ViewComponents;
using SportsStore.Components;
using SportsStore.Models;
using System.Linq;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests 
    {
        [Fact]
        public void CanGetCategories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category="ACategory"},
                new Product {ProductID = 2, Name = "P2", Category ="BCategory"},
                new Product {ProductID = 3, Name = "P3", Category="CCategory"},
                new Product {ProductID = 4, Name = "P4", Category="ACategory"},
                new Product {ProductID = 5, Name = "P5", Category="CCategory"}
            }).AsQueryable<Product>());

            var Menu = new NavigationMenuViewComponent(mock.Object);

            string[] result = ((Menu.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<string>).ToArray();

            Assert.Equal("ACategory", result[0]);
            Assert.Equal("BCategory", result[1]);
            Assert.Equal("CCategory", result[2]);
        }

        [Fact]
        public void CanGetSelectedCategory()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category="ACategory"},
                new Product {ProductID = 2, Name = "P2", Category ="BCategory"},
                new Product {ProductID = 3, Name = "P3", Category="CCategory"},
                new Product {ProductID = 4, Name = "P4", Category="ACategory"},
                new Product {ProductID = 5, Name = "P5", Category="CCategory"}
            }).AsQueryable<Product>());

            var Menu = new NavigationMenuViewComponent(mock.Object);

            Menu.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext { RouteData = new Microsoft.AspNetCore.Routing.RouteData() }
            };

            Menu.RouteData.Values["category"] = "ACategory";

            string result = (Menu.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"] as string;

            Assert.Equal("ACategory", result);

        }
    }
}
