using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movies.Interfaces;
using Movies.DataContracts;

namespace Movies.DataAccess
{
    public class MovieCache: IMovieCache
    {
        private static string _cacheKey = "$movies";
        private static object _lockInstance = new object();
        private static MovieCache _instance;

        public static MovieCache Instance
        {
            get 
            {
                if (_instance == null) 
                {
                    lock (_lockInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = new MovieCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private object _lockObj = new object();

        private MovieCache() 
        {
        }
        
        public bool IsEmpty 
        {
            get {
                return HttpContext.Current.Cache[_cacheKey] == null;
            }
        }

        public IEnumerable<Movie> GetMovies()
        {
            if (this.IsEmpty) {
                return new List<Movie>();
            }
            return HttpContext.Current.Cache[_cacheKey] as List<Movie>;
        }

        public void PutMovies(IEnumerable<Movie> list)
        {
            lock (_lockObj)
            {
                HttpContext.Current.Cache[_cacheKey] = new List<Movie>(list);
            }
        }

        public void AddMovie(Movie movie)
        {
            lock (_lockObj)
            {
                if (this.IsEmpty) 
                {
                    this.PutMovies(new Movie[] { movie });
                    return;
                }
                var movies = this.GetMovies().ToList();
                movies.Add(movie);
                this.PutMovies(movies);
            }
        }

        public void UpdateMovie(Movie movie)
        {
            lock (_lockObj)
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException("Update operation on empty cache cannot be performed.");
                }
                var movies = this.GetMovies();
                var cachedMovie = movies.First(m => m.Id == movie.Id);

                cachedMovie.Genre = movie.Genre;
                cachedMovie.Title = movie.Title;
                cachedMovie.Rating = movie.Rating;
                cachedMovie.ReleaseYear = movie.ReleaseYear;
                cachedMovie.Classification = movie.Classification;
                cachedMovie.Cast = new string[movie.Cast.Length];
                movie.Cast.CopyTo(cachedMovie.Cast, 0);
                this.PutMovies(movies);
            }
        }

        public Movie GetMovieById(int id)
        {
            if (this.IsEmpty) 
            {
                return null;
            }
            return this.GetMovies().FirstOrDefault(m => m.Id == id);
        }
    }
}