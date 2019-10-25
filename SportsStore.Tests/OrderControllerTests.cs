using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Controllers;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact] // не сохраняет заказы с пустой корзиной 
        public void CannotCheckoutEmptyCart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();

            Order order = new Order();

            OrderController target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            // Проврека что заказ не был сохранен
            // (метод сохранения заказа никогда не вызывается)
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // проверка что метод вовзращяет стандартное представление
            // (представление не имеет имени)
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            // проверка что представлению передана невалидная модель 
            Assert.False(result.ViewData.ModelState.IsValid);
        }


        [Fact] // не сохраняет невалидные заказы 
        public void CannotCheckoutInvalidShoppingDetails()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();

            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            // добавление ошыбки в модель 
            target.ModelState.AddModelError("error", "error");

            // попытка перехода к оплате невалидного ордера 
            ViewResult result = target.Checkout(new Order()) as ViewResult;


            // проверка 
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.True(string.IsNullOrEmpty(result.ViewName));

            Assert.False(result.ViewData.ModelState.IsValid);
        }


        [Fact]// сохраняет валидный ордер с непустой корзиной 
        public void CanCheckoutAndSubmitOroder()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            cart.AddItem(new Product { Name = "TestProduct", ProductID = 0 }, 1);

            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            RedirectToActionResult result = target.Checkout(order) as RedirectToActionResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            Assert.Equal("Completed", result.ActionName);
        }
    }
}
