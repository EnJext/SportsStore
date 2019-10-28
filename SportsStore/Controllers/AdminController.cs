using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AdminController: Controller
    {
        private IProductRepository products;
        public AdminController(IProductRepository repo) => products = repo;
        
        public ViewResult Index() => View(products.Products);

        public ViewResult Edit(int ProductID) => View(products.Products.FirstOrDefault(p => p.ProductID == ProductID));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                products.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create() => View("Edit", new Product());

        public IActionResult Delete(int ProductID)
        {
            Product deletedProduct = products.DeleteProduct(ProductID);
            if(deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
