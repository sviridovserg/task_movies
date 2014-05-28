using System.Collections.Generic;
using Movies.DataContracts;

namespace Movies.Interfaces
{
    public interface IDataSourceAdapter
    {
        void Create(Movie movie);
        List<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        void Update(Movie movie);
    }
}