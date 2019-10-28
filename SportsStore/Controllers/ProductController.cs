using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public int PageSize = 4;
        public IActionResult List(string category, int productPage=1)
        {
            IEnumerable<Product> products = repository.Products
                .Where(p => p.Category == category || category == null)
                .OrderBy(p => p.ProductID);

            ProductsListViewModel Model = new ProductsListViewModel
            {
                Products = products.Skip((productPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemPerPage = PageSize,
                    TotalItems = products.Count(),
                },
                CurrentCategory = category
            };
            return View(Model);
        }

    }
}