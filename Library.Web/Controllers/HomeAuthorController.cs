using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class HomeAuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
