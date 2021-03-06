﻿using System.Collections.Generic;
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

        public int Create(Movie movie)
        {
            return _dataSource.Create((MovieData) movie);
        }

        public List<Movie> GetAllMovies()
        {
            List<MovieData> result = _dataSource.GetAllData();
            if (result == null || result.Count == 0)
            {
                return new List<Movie>();
            }
            return result.Select(x => (Movie) x).ToList();
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