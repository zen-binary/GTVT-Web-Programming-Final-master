using Microsoft.AspNetCore.Mvc;

namespace Project_Final.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ILogger<OverviewController> _logger;

        public OverviewController(ILogger<OverviewController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
