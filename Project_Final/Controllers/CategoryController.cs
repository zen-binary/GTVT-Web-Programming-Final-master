using Microsoft.AspNetCore.Mvc;
using Project_Final.Models.Entity;
using Project_Final.Models.Response;
using Project_Final.Services;

namespace Project_Final.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService category)
        {
            _categoryService = category;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
