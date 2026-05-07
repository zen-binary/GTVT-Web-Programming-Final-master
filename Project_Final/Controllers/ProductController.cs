using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Final.Context;
using Project_Final.Models;
using Project_Final.Services;

namespace Project_Final.Controllers
{
    public class ProductController : Controller
    {
        private readonly DBContext _dbContext;

        public ProductController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_dbContext.Categories.Where(c => c.Status == Status.ACTIVE).ToList(), "Id", "Name");
            return View();
        }

        public IActionResult Edit(long id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var catProduct = _dbContext.Categories.Find(product.CategoryId);
            var activeCategories = _dbContext.Categories.Where(c => c.Status == Status.ACTIVE).ToList();

            if (catProduct != null && !activeCategories.Any(c => c.Id == catProduct.Id))
            {
                activeCategories.Add(catProduct);
            }

            ViewBag.CategoryID = new SelectList(activeCategories, "Id", "Name");
            return View(product);
        }

    }
}
