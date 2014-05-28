using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movies.Interfaces;
using Movies.DataContracts;

namespace Movies.DataAccess
{
	public class MoviesDataAccess: IMoviesDataAccess
	{
		public Movie[] GetAllMovies(DataContracts.SortDescription sort)
		{
			throw new NotImplementedException();
		}

		public Movie[] SearchMovies(DataContracts.SortDescription sort)
		{
			throw new NotImplementedException();
		}

		public void AddMovie(Movie movie)
		{
			throw new NotImplementedException();
		}

		public void UpdateMovie(Movie movie)
		{
			throw new NotImplementedException();
		}
	}
}