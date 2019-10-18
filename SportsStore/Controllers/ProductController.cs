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
        public IActionResult List(int productPage=1)
        {
            return View(new ProductsListViewModel
            {
                Products = repository.Products.Skip((productPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            });
        }

    }
}