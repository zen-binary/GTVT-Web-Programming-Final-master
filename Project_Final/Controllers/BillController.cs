using Microsoft.AspNetCore.Mvc;

namespace Project_Final.Controllers
{
    public class BillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(long id)
        {
            return View(id);
        }
    }
}
