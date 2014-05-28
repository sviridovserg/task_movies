using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Movies.DataContracts;

namespace Movies.Interfaces
{
	interface IMoviesDataAccess
	{
		Movie[] GetAllMovies(SortDescription sort);
		Movie[] SearchMovies(SortDescription sort);
		void AddMovie(Movie movie);
		void UpdateMovie(Movie movie);
	}
}
