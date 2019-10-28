using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private Cart cart;
        private IOrderRepository repository;

        public OrderController(IOrderRepository repo , Cart crt)
        {
            cart = crt;
            repository = repo;
        }

        // страница оформления заказа 
        public ViewResult Checkout() => View(new Order());

        [HttpPost]  // обработка заказа 
        public IActionResult Checkout(Order order)
        {
            // проверяем корзину на наличие товара 
            if(cart.Lines.Count() ==0)
            {
                ModelState.AddModelError("", "Sorry your cart is empty");
            }

            //проверяем валидность модели 
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                // отправка модели обратно в случае если модель не валидна
                return View(order);
            }
        }

        // успешное оформление заказа 
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(order=>!order.Shipped).OrderBy(order => order.OrderID));

        [Authorize]
        public IActionResult MarkShipped(int OrderID)
        {
            Order order = repository.Orders.Where(or => or.OrderID == OrderID).FirstOrDefault();
            if(order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }


    }
}
