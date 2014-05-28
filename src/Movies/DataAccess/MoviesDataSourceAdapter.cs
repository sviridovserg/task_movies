using System;
using System.Collections.Generic;
using System.Linq;
using Movies.DataContracts;
using Movies.Interfaces;
using MoviesLibrary;

namespace Movies.DataAccess
{
    public sealed class MoviesDataSourceAdapter : IDataSourceAdapter
    {
        private readonly MovieDataSource _dataSource;

        public MoviesDataSourceAdapter()
        {
            _dataSource = new MovieDataSource();
        }

        public void Create(Movie movie)
        {
            _dataSource.Create((MovieData) movie);
        }

        public List<Movie> GetAllMovies()
        {
            List<MovieData> result = _dataSource.GetAllData();
            if (result == null || result.Count == 0)
            {
                return new List<Movie>();
            }
#warning почему бы не сделать так, тогда не надо создавать converter ?
            return result.Select(x => (Movie) x).ToList();
            return result.ConvertAll(new Converter<MovieData, Movie>(data => data)).ToList();
        }

        public Movie GetMovieById(int id)
        {
            return _dataSource.GetDataById(id);
        }

        public void Update(Movie movie)
        {
            _dataSource.Update((MovieData) movie);
        }
    }
}