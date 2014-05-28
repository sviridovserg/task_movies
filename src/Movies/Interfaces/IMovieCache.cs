using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movies.DataContracts;

namespace Movies.Interfaces
{
    public interface IMovieCache
    {
        bool IsEmpty { get; }
        IEnumerable<Movie> GetMovies();
        void PutMovies(IEnumerable<Movie> list);
        void AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        Movie GetMovieById(int id);
    }
}