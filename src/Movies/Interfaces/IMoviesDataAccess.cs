using System.Collections.Generic;
using Movies.DataContracts;
using Movies.Models;

namespace Movies.Interfaces
{
    internal interface IMoviesDataAccess
    {
        IEnumerable<Movie> GetAllMovies(string field = "", SortDirection? direction = null);
        IEnumerable<Movie> SearchMovies(string field, string expression);
        void AddMovie(Movie movie);
        void UpdateMovie(Movie movie);
        Movie GetMovieById(string id);
    }
}