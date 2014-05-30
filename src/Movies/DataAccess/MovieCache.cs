using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Movies.DataContracts;
using Movies.Interfaces;
using System.ServiceModel;
using Movies.DataContracts.FaultContracts;

namespace Movies.DataAccess
{
    public sealed class MovieCache : IMovieCache
    {
        private static readonly string _cacheKey = "$movies";

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


        public bool IsInitialized
        {
            get { return _cache[_cacheKey] != null; }
        }

        public IEnumerable<Movie> GetMovies()
        {
			CheckInitialized("GetMovies operation failed. ");
            
            return _cache[_cacheKey] as List<Movie>;
        }

        public void PutMovies(IEnumerable<Movie> list)
        {
            lock (_syncObject)
            {
                foreach (Movie m in list) 
                {
                    m.CacheId = this.GetElementId();
                }
                _cache[_cacheKey] = new List<Movie>(list);
            }
        }

        public void AddMovie(Movie movie)
        {
            lock (_syncObject)
            {
                CheckInitialized("Add operation failed. ");
                
                movie.CacheId = this.GetElementId();
                List<Movie> movies = GetMovies().ToList();
                movies.Add(movie);
                PutMovies(movies);
            }
        }

        public void UpdateMovie(Movie movie)
        {
            lock (_syncObject)
            {
                CheckInitialized("Update operation failed. ");
                
                IEnumerable<Movie> movies = GetMovies();
                Movie cachedMovie = movies.First(m => m.CacheId == movie.CacheId);

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
            CheckInitialized("GetMovieById operation failed. ");
            
            return GetMovies().FirstOrDefault(m => m.Id == id);
        }

        private void CheckInitialized(string operation) 
        {
            if (!IsInitialized)
            {
                throw new FaultException<InitializationFault>(new InitializationFault(operation + "Cache is not initialized"));
            }
        }

        private string GetElementId() {
            return Guid.NewGuid().ToString();
        }
    }
}