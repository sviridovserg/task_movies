using System;
using System.ServiceModel.Activation;
using Movies.DataAccess;
using Movies.DataContracts;
using Movies.Interfaces;
using Movies.Models;
using Movies.Utils;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using Movies.DataContracts.FaultContracts;

namespace Movies
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
#warning you have to add FailureInfo class to get Exceptions accross Service boundary.
#warning you have to specify what kind of exceptions your service can throw
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class MoviesService : IMoviesService
    {
        private readonly int MERGE_TIMER = 30000;
        private readonly IMoviesDataAccess _dataAccess;
        private readonly Timer _mergeTimer;
        private readonly IDataMerger _merger;

        public MoviesService()
        {
            IDataSourceAdapter dataSource = new MoviesDataSourceAdapter();
            ILogger logger = new Logger();
            _dataAccess = new MoviesDataAccess(MovieCache.Instance, dataSource, logger);
            
            _merger = new DataMerger(MovieCache.Instance, dataSource, logger);
            _mergeTimer = new Timer(MERGE_TIMER);
            _mergeTimer.Elapsed += new ElapsedEventHandler(_mergeTimer_Elapsed);
            _mergeTimer.Start();
        }

        private void _mergeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _merger.MergeCacheIntoDatsource();
        }

        [FaultContract(typeof(InitializationFault))]
        public Movie[] GetList(string field, string direction)
        {
            SortDirection sortDirection;
            if (Enum.TryParse(direction, true, out sortDirection))
            {
                return _dataAccess.GetAllMovies(field, sortDirection).ToArray();
            }
            return _dataAccess.GetAllMovies().ToArray();
        }

        [FaultContract(typeof(InitializationFault))]
        public Movie[] Search(string field, string expression)
        {
            return _dataAccess.SearchMovies(field, expression).ToArray();
        }

        [FaultContract(typeof(InitializationFault))]
        public void UpdateMovie(Movie movie)
        {
            _dataAccess.UpdateMovie(movie);
        }

        [FaultContract(typeof(InitializationFault))]
        public void AddMovie(Movie movie)
        {
            _dataAccess.AddMovie(movie);
        }

        [FaultContract(typeof(InitializationFault))]
        public Movie GetMovie(string id)
        {
            int movieId;
            if (!int.TryParse(id, out movieId))
            {
                return null;
            }
            return _dataAccess.GetMovieById(movieId);
        }
    }
}