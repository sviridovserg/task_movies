using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movies.Interfaces;
using Movies.DataContracts;

namespace Movies.Utils
{
    public class DataMerger: IDataMerger
    {
        private IMovieCache _cache;
        private IDataSourceAdapter _dataSource;
        private ILogger _logger;

        public DataMerger(IMovieCache cache, IDataSourceAdapter dataSource, ILogger logger) 
        {
            _cache = cache;
            _dataSource = dataSource;
            _logger = logger;
        }

        public void MergeCacheIntoDatsource() 
        {
            try
            {
                IList<Movie> dataSourceMovies = _dataSource.GetAllMovies();
                IList<Movie> cachedMovies = _cache.GetMovies().ToList();

                IEnumerable<Movie> addedMovies = cachedMovies.Where(m => m.Id == 0);
                IEnumerable<Movie> updatedMovies = dataSourceMovies.Where(m => !cachedMovies.Contains(m));

                foreach (var added in addedMovies) 
                {
                    _dataSource.Create(added);
                }
                foreach (var updated in updatedMovies) 
                {
                    _dataSource.Update(updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to merge data into data source", ex);
            }
        }
    }
}