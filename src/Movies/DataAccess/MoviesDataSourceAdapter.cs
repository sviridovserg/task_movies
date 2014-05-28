using System.Collections.Generic;
using System.Linq;
using Movies.DataContracts;
using Movies.Interfaces;
using MoviesLibrary;


namespace Movies.DataAccess
{
    public class MoviesDataSourceAdapter: IDataSourceAdapter
    {
        private MovieDataSource _dataSource;
        
        public MoviesDataSourceAdapter() 
        {
            _dataSource = new MovieDataSource();
        }

        public void Create(Movie movie)
        {
            _dataSource.Create((MovieData)movie);
        }

        public List<Movie> GetAllData()
        {
            List<MovieData> result = _dataSource.GetAllData();
            if (result == null || result.Count == 0) 
            {
                return new List<Movie>();
            }
            return result.ConvertAll<Movie>(new System.Converter<MovieData,Movie>(data => data)).ToList();
        }

        public Movie GetDataById(int id)
        {
            return _dataSource.GetDataById(id);
        }

        public void Update(Movie movie)
        {
            _dataSource.Update((MovieData)movie);
        }
    }
}