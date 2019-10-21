﻿using Microsoft.AspNetCore.Mvc;
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

        public CartController(IProductRepository repo) => repository = repo;

        public ViewResult Index(string returnUrl) => View(new CartIndexViewModel
        {
            Cart = GetCart(),
            ReturnUrl = returnUrl
        });
        [HttpPost]
        public RedirectToActionResult AddToCart(string returnUrl, int ProductID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            if(product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(string returnUrl, int ProductID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            if(product !=null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }


        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart) => HttpContext.Session.SetJson("Cart", cart);

    }
}
