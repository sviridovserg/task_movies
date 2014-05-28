using Movies.DataContracts;
using Movies.Models;

namespace Movies.Interfaces
{
	interface IMoviesDataAccess
	{
		Movie[] GetAllMovies(string field = "", SortDirection? direction = null);
        Movie[] SearchMovies(string field, string expression);
		void AddMovie(Movie movie);
		void UpdateMovie(Movie movie);
        Movie GetMovieById(int id);
	}
}
