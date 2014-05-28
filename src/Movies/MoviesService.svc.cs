using System;
using System.ServiceModel.Activation;
using Movies.DataAccess;
using Movies.Interfaces;
using Movies.Models;
using Movies.Utils;

namespace Movies
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class MoviesService : IMoviesService
	{
        private IMoviesDataAccess _dataAccess;
        public MoviesService() 
        {
            _dataAccess = new MoviesDataAccess(MovieCache.Instance, new MoviesDataSourceAdapter(), new Logger());
        }

        public DataContracts.Movie[] GetList(string field, string direction)
        {
            SortDirection sortDirection;
            if (Enum.TryParse<SortDirection>(direction, true, out sortDirection)) 
            {
               return  _dataAccess.GetAllMovies(field, sortDirection);
            }
            return _dataAccess.GetAllMovies();
        }

        public DataContracts.Movie[] Search(string field, string expression)
        {
            return _dataAccess.SearchMovies(field, expression);
        }

        public void UpdateMovie(DataContracts.Movie movie)
        {
            _dataAccess.AddMovie(movie);
        }

        public void AddMovie(DataContracts.Movie movie)
        {
            _dataAccess.UpdateMovie(movie);
        }

        public DataContracts.Movie GetMovie(string id) 
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
