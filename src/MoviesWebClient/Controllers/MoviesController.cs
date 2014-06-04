using System.ServiceModel;
using System.Web.Mvc;
using Movies;
using Movies.DataContracts;

namespace MoviesWebClient.Controllers
{
    public class MoviesController : Controller
    {
        //
        // GET: /Movies/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult AllMovies()
        {
            return Json(GetMoviesService().GetList(null, null), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Search(string field, string expression)
        {
            return Json(GetMoviesService().Search(field, expression), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetMovie(string id)
        {
            return Json(GetMoviesService().GetMovie(id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public void UpdateMovie(Movie movie)
        {
            GetMoviesService().UpdateMovie(movie);
        }

        [Authorize]
        [HttpPost]
        public void AddMovie(Movie movie)
        {
            GetMoviesService().AddMovie(movie);
        }

        private IMoviesService GetMoviesService()
        {
            var channelFactory = new ChannelFactory<IMoviesService>("MoviesServiceEndpoint");
            return channelFactory.CreateChannel();
        }
    }
}