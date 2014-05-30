using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Movies.DataContracts;
using Movies.Interfaces;
using Movies.Models;

namespace Movies.DataAccess
{
    public sealed class MoviesDataAccess : IMoviesDataAccess
    {
        private readonly IMovieCache _cache;
        private readonly IDataSourceAdapter _dataSource;
        private readonly ILogger _logger;

        public MoviesDataAccess(IMovieCache cache, IDataSourceAdapter dataSource, ILogger logger)
        {
            _cache = cache;
            _dataSource = dataSource;
            _logger = logger;
            _cache.PutMovies(_dataSource.GetAllMovies());
        }

        public IEnumerable<Movie> GetAllMovies(string field = "", SortDirection? sortDirection = null)
        {
            List<Movie> result = _cache.GetMovies().ToList();
            if (!string.IsNullOrEmpty(field) && sortDirection.HasValue)
            {
                result.Sort(new MovieComparer(field, sortDirection.Value));
            }
            return result;
        }
        public IEnumerable<Movie> SearchMovies(string field, string expression)
        {
			return _cache.GetMovies().Where(GetSearchPredicate(field, expression));
        }

        public void AddMovie(Movie movie)
        {
            _cache.AddMovie(movie);
        }

        public void UpdateMovie(Movie movie)
        {
            _cache.UpdateMovie(movie);
        }

        public Movie GetMovieById(int id)
        {
            Movie result = _cache.GetMovieById(id);
            return result;
        }


        private static Func<Movie, bool> GetSearchPredicate(string field, string expression)
        {
			if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(expression))
			{
				return m => true;
			}
            Type movieType = typeof (Movie);
            PropertyInfo propertyDescriptor = movieType.GetProperty(field);
            if (propertyDescriptor == null) 
            {
                return m => true;
            }
            return m =>
                       {
                           object propertyValue = propertyDescriptor.GetValue(m, new object[] {});
                           if (propertyValue == null)
                           {
                               return false;
                           }
                           if (propertyDescriptor.PropertyType == typeof (int))
                           {
                               int restriction;
                               if (int.TryParse(expression, out restriction))
                               {
                                   return Convert.ToInt32(propertyValue) == restriction;
                               }
                               return false;
                           }
                           else if (propertyDescriptor.PropertyType == typeof (string))
                           {
							   return propertyValue.ToString().ToUpper().Contains(expression.ToUpper());
                           }
                           else if (propertyDescriptor.PropertyType == typeof (string[]))
                           {
                               var arrayVal = propertyValue as string[];
							   return arrayVal.FirstOrDefault(s => s.ToUpper().Contains(expression.ToUpper())) != null;
                           }
                           return false;
                       };
        }
    }
}