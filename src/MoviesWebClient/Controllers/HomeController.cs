using System.Web.Mvc;

namespace MoviesWebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //User.Identity.Name
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Movies");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [Authorize]
        public ActionResult Movies()
        {
            ViewBag.Message = "Moviews.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}