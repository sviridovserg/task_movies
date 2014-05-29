using System;
using System.Collections.Generic;
using System.Linq;
using Movies.DataContracts;
using Movies.Interfaces;
using Movies.Models;

namespace Movies.DataAccess
{
	public class MoviesDataAccess: IMoviesDataAccess
	{
        private IMovieCache _cache;
        private ILogger _logger;
        private IDataSourceAdapter _dataSource;

        public MoviesDataAccess(IMovieCache cache, IDataSourceAdapter dataSource, ILogger logger) 
        {
            _cache = cache;
            _dataSource = dataSource;
        }

        private List<Movie> GetMovies() 
        {
            List<Movie> result = null;
            try
            {
                if (_cache.IsEmpty)
                {
                    result = _dataSource.GetAllData();
                    _cache.PutMovies(result);
                }
                else
                {
                    result = _cache.GetMovies().ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Faild to get movies", ex);
            }
            return result;
        }

        private Func<Movie, bool> GetSearchPredicate(string field, string expression) 
        {
            Type movieType = typeof(Movie);
            System.Reflection.PropertyInfo propertyDescriptor = movieType.GetProperty(field);
            return m =>
            {
                object propertyValue = propertyDescriptor.GetValue(m, new object[] { });
                if (propertyValue == null)
                {
                    return false;
                }
                if (propertyDescriptor.PropertyType == typeof(int))
                {
                    int restriction;
                    if (int.TryParse(expression, out restriction))
                    {
                        return Convert.ToInt32(propertyValue) == restriction;
                    }
                    return false;
                }
                else if (propertyDescriptor.PropertyType == typeof(string))
                {
                    return propertyValue.ToString().ToLower().Contains(expression.ToLower());
                }
                else if (propertyDescriptor.PropertyType == typeof(string[]))
                {
                    var arrayVal = propertyValue as string[];
                    return arrayVal.FirstOrDefault(s => s.ToLower().Contains(expression.ToLower())) != null;
                }
                return false;
            };
        }

		public Movie[] GetAllMovies(string field = "", SortDirection? sortDirection = null)
		{
            List<Movie> result = GetMovies();
            if (!string.IsNullOrEmpty(field) && sortDirection.HasValue)
            {
                result.Sort(new MovieComparer(field, sortDirection.Value));
            }
            return result.ToArray();
		}

		public Movie[] SearchMovies(string field, string expression)
		{
            return GetMovies().Where(this.GetSearchPredicate(field, expression)).ToArray();
		}

		public void AddMovie(Movie movie)
		{
            _cache.AddMovie(movie);
            try
            {
                int id = _dataSource.Create(movie);
                movie.Id = id;
            }
            catch (Exception ex) 
            {
                _logger.LogError("Faild to add movie to movies data source", ex);
            }
		}

		public void UpdateMovie(Movie movie)
		{
            _cache.UpdateMovie(movie);
            try
            {
                _dataSource.Update(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError("Faild to update movie information in movies data source", ex);
            }
		}

        public Movie GetMovieById(int id)
        {
            var result = _cache.GetMovieById(id);
            if (result == null)
            {
                try
                {
                    result = _dataSource.GetDataById(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Faild to get movie from movie data source", ex);
                }
            }
            return result;

        }
	}
}