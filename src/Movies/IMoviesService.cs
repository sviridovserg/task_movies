﻿using System.ServiceModel;
using System.ServiceModel.Web;
using Movies.DataContracts;

namespace Movies
{
    [ServiceContract]
    public interface IMoviesService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "movies?sort_field={field}&sort_direction={direction}")]
        Movie[] GetList(string field, string direction);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "movies/search?field={field}&expression={expression}")]
        Movie[] Search(string field, string expression);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "movies/update")]
        void UpdateMovie(Movie movie);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "movies/add")]
        void AddMovie(Movie movie);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "movies/{id}")]
        Movie GetMovie(string id);
    }
}