using System;
using System.ServiceModel.Activation;
using Movies.DataAccess;
using Movies.DataContracts;
using Movies.Interfaces;
using Movies.Models;
using Movies.Utils;

namespace Movies
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
#warning you have to add FailureInfo class to get Exceptions accross Service boundary.
#warning you have to specify what kind of exceptions your service can throw
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesDataAccess _dataAccess;

        public MoviesService()
        {
            _dataAccess = new MoviesDataAccess(MovieCache.Instance, new MoviesDataSourceAdapter(), new Logger());
        }

        public Movie[] GetList(string field, string direction)
        {
            SortDirection sortDirection;
            if (Enum.TryParse(direction, true, out sortDirection))
            {
                return _dataAccess.GetAllMovies(field, sortDirection);
            }
            return _dataAccess.GetAllMovies();
        }

        public Movie[] Search(string field, string expression)
        {
            return _dataAccess.SearchMovies(field, expression);
        }

        public void UpdateMovie(Movie movie)
        {
            _dataAccess.AddMovie(movie);
        }

        public void AddMovie(Movie movie)
        {
            _dataAccess.UpdateMovie(movie);
        }

        public Movie GetMovie(string id)
        {
            int movieId;
            if (!int.TryParse(id, out movieId))
            {
                return null;
            }
            return _dataAccess.GetMovieById(movieId);
        }
    }
}