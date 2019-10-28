using System;
using System.Collections.Generic;
using SportsStore.Models;
using Moq;
using Xunit;
using System.Linq;

namespace SportsStore.Tests
{
    public class CartControllerTests
    {
        [Fact]
        public void CanAddItems()
        {
            CartLine[] result = GetCart().Lines.OrderBy(p=>p.Product.Price).ToArray();

            Assert.Equal(2, result.Length);
            Assert.Equal("P1", result[0].Product.Name);
            Assert.Equal("P2", result[1].Product.Name);
        }

        [Fact]
        public void CanQuantityItems()
        {
            CartLine[] result = GetCart().Lines.OrderBy(p => p.Product.Price).ToArray();

            Assert.Equal(2, result[0].Quantity);
            Assert.Equal(2, result[1].Quantity);
        }

        [Fact]
        public void CanRemoveItems()
        {
            Cart target = new Cart();
            Product product1 = new Product { Name = "P2", Price = 140 };
            Product product2 = new Product { Name = "P1", Price = 100 };

            target.AddItem(product2, 2);
            target.AddItem(product1, 1);
            target.AddItem(product1, 1);

            target.RemoveLine(product1);

            CartLine[] result = target.Lines.OrderBy(p => p.Product.Price).ToArray();

            Assert.Single(result);
        }


        [Fact]
        public void CanComputeTotalValue()
        {
            Cart target = GetCart();

            decimal result = target.ComputeTotalValue();

            Assert.Equal(480, result);
        }

        [Fact]
        public void CanClearAllLines()
        {
            Cart target = GetCart();

            target.Clear();
            CartLine[] result = target.Lines.OrderBy(p => p.Product.Price).ToArray();

            Assert.Empty(result);
        }

        private Cart GetCart()
        {
            Cart target = new Cart();
            Product product1 = new Product { Name = "P2", Price = 140 };
            Product product2 = new Product { Name = "P1", Price = 100 };

            target.AddItem(product2, 2);
            target.AddItem(product1, 1);
            target.AddItem(product1, 1);

            return target;
        }
    }
}
