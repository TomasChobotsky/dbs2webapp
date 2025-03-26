using Microsoft.AspNetCore.Mvc;

namespace dbs2webapp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
