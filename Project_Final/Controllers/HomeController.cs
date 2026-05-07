using Microsoft.AspNetCore.Mvc;
using Project_Final.Models;
using System.Diagnostics;

namespace Project_Final.Controllers
{
    [Route("/Admin/[controller]")]
    public class HomeController : Controller
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
