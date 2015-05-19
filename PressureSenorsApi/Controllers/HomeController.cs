using System.Web.Mvc;

namespace PressureSensorsApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
