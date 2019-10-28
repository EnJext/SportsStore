using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController :Controller
    {
        private IProductRepository repository;
        private Cart cart;
        public CartController(IProductRepository repo, Cart cart)
        {
            this.cart = cart;
            repository = repo;
        }

        public ViewResult Index(string returnUrl) => View(new CartIndexViewModel
        {
            Cart = cart,
            ReturnUrl = returnUrl
        });

        [HttpPost]
        public RedirectToActionResult AddToCart(string returnUrl, int ProductID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            if(product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public RedirectToActionResult RemoveFromCart(string returnUrl, int ProductID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            if(product !=null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

    }
}
