using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Movies.DataContracts;
using Movies.Interfaces;

namespace Movies.DataAccess
{
    public sealed class MovieCache : IMovieCache
    {
        private static readonly string _cacheKey = "$movies";

#warning HttpContext.Current can be null (!) I suggest you inject or bypass it somehow
        private static readonly MovieCache _instance = new MovieCache(HttpContext.Current.Cache);

        private readonly System.Web.Caching.Cache _cache;
        private readonly object _syncObject = new object();

        private MovieCache(Cache cache)
        {
            _cache = cache;
        }

        public static MovieCache Instance
        {
            get
            {
                return _instance;
            }
        }


        public bool IsEmpty
        {
            get { return _cache[_cacheKey] == null; }
        }

        public IEnumerable<Movie> GetMovies()
        {
            if (IsEmpty)
            {
                return new List<Movie>();
            }
            return _cache[_cacheKey] as List<Movie>;
        }

        public void PutMovies(IEnumerable<Movie> list)
        {
            lock (_syncObject)
            {
                _cache[_cacheKey] = new List<Movie>(list);
            }
        }

        public void AddMovie(Movie movie)
        {
            lock (_syncObject)
            {
                if (IsEmpty)
                {
                    PutMovies(new[] {movie});
                    return;
                }
                List<Movie> movies = GetMovies().ToList();
                movies.Add(movie);
                PutMovies(movies);
            }
        }

        public void UpdateMovie(Movie movie)
        {
            lock (_syncObject)
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Update operation on empty cache cannot be performed.");
                }
                IEnumerable<Movie> movies = GetMovies();
                Movie cachedMovie = movies.First(m => m.Id == movie.Id);

                cachedMovie.Genre = movie.Genre;
                cachedMovie.Title = movie.Title;
                cachedMovie.Rating = movie.Rating;
                cachedMovie.ReleaseYear = movie.ReleaseYear;
                cachedMovie.Classification = movie.Classification;
                cachedMovie.Cast = new string[movie.Cast.Length];
                movie.Cast.CopyTo(cachedMovie.Cast, 0);
                PutMovies(movies);
            }
        }

        public Movie GetMovieById(int id)
        {
            if (IsEmpty)
            {
                return null;
            }
            return GetMovies().FirstOrDefault(m => m.Id == id);
        }
    }
}