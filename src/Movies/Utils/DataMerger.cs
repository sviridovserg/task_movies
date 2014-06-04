using System;
using System.Collections.Generic;
using System.Linq;
using Movies.DataContracts;
using Movies.Interfaces;

namespace Movies.Utils
{
    public class DataMerger : IDataMerger
    {
        private readonly IMovieCache _cache;
        private readonly IDataSourceAdapter _dataSource;
        private readonly ILogger _logger;

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

                IEnumerable<Movie> addedMovies = cachedMovies.Where(m => m.IsNew());
                IEnumerable<Movie> updatedMovies = cachedMovies.Where(m => !m.IsNew() && !dataSourceMovies.Contains(m));

                foreach (Movie added in addedMovies)
                {
                    added.Id = _dataSource.Create(added);
                }
                foreach (Movie updated in updatedMovies)
                {
                    _dataSource.Update(updated);
                }
                _cache.PutMovies(cachedMovies);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to merge data into data source", ex);
            }
        }
    }
}