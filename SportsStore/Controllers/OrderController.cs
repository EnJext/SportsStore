using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

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


    }
}
