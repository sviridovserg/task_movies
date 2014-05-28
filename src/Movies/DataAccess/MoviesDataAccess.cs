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
#warning почему не возвращаешь IList<Movie> ?
        public Movie[] SearchMovies(string field, string expression)
        {
            return GetMovies().Where(GetSearchPredicate(field, expression)).ToArray();
        }

        public void AddMovie(Movie movie)
        {
#warning same Moview might allready exist!
            _cache.AddMovie(movie);
            try
            {
                _dataSource.Create(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError("Faild to add movie to movies data source", ex);
            }
        }

        public void UpdateMovie(Movie movie)
        {
#warning cannot update moview without checking if it already had been updated or deleted!
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
            Movie result = _cache.GetMovieById(id);
            if (result == null)
            {
                try
                {
                    result = _dataSource.GetMovieById(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Faild to get movie from movie data source", ex);
                }
            }
            return result;
        }

        private List<Movie> GetMovies()
        {
            List<Movie> result = null;
            try
            {
                if (_cache.IsEmpty)
                {
                    result = _dataSource.GetAllMovies();
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

#warning - тут большая дыра, так как нет валидации expression. Хотя бы оберни это в try/catch и выкидывай наружу какое-то своё нормальное исключение
        private static Func<Movie, bool> GetSearchPredicate(string field, string expression)
        {
            Type movieType = typeof (Movie);
            PropertyInfo propertyDescriptor = movieType.GetProperty(field);
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
#warning насколько я знаю, алгоритм сравнения строк ToUpper чуток эффективнее
                               return propertyValue.ToString().ToLower().Contains(expression.ToLower());
                           }
                           else if (propertyDescriptor.PropertyType == typeof (string[]))
                           {
                               var arrayVal = propertyValue as string[];
#warning arrayVal can be null
                               return arrayVal.FirstOrDefault(s => s.ToLower().Contains(expression.ToLower())) != null;
                           }
                           return false;
                       };
        }
    }
}