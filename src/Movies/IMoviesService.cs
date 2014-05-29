using System.ServiceModel;
using System.ServiceModel.Web;
using Movies.DataContracts;

namespace Movies
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IMoviesService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "json/movies?sort_field={field}&sort_direction={direction}")]
        Movie[] GetList(string field, string direction);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "json/movies/search?field={field}&expression={expression}")]
        Movie[] Search(string field, string expression);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/movies/update")]
        void UpdateMovie(Movie movie);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/movies/add")]
        void AddMovie(Movie movie);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/movies/{id}")]
        Movie GetMovie(string id);
    }
}