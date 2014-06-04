using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Movies.DataAccess;
using Movies.DataContracts;
using Movies.Interfaces;
using Movies.Models;

namespace MoviesUnitTests
{
    [TestClass]
    public class MoviesDataAccessTest
    {
        private readonly Mock<IMovieCache> _movieCacheMock = new Mock<IMovieCache>();
        private IMoviesDataAccess _moviesDataAccess;

        [TestInitialize]
        public void TestInit()
        {
            var dataSourceAdapterMock = new Mock<IDataSourceAdapter>();

            dataSourceAdapterMock.Setup(d => d.GetAllMovies()).Returns(GetPredefinedMovies());
            _moviesDataAccess = new MoviesDataAccess(_movieCacheMock.Object, dataSourceAdapterMock.Object,
                                                     new Mock<ILogger>().Object);
        }

        private List<Movie> GetPredefinedMovies()
        {
            var allMovies = new List<Movie>
                                {
                                    new Movie
                                        {
                                            Id = 1,
                                            Classification = "M",
                                            Genre = "Action",
                                            Rating = 1,
                                            ReleaseYear = 2014,
                                            Title = "Movie1",
                                            Cast = new[] {"A1", "A2"}
                                        },
                                    new Movie
                                        {
                                            Id = 2,
                                            Classification = "M1",
                                            Genre = "Comedy",
                                            Rating = 3,
                                            ReleaseYear = 2010,
                                            Title = "Movie2",
                                            Cast = new string[] {}
                                        },
                                    new Movie
                                        {
                                            Id = 3,
                                            Classification = "M2",
                                            Genre = "Cartoon",
                                            Rating = 3,
                                            ReleaseYear = 2012,
                                            Title = "Movie3",
                                            Cast = null
                                        },
                                    new Movie
                                        {
                                            Id = 4,
                                            Classification = "M",
                                            Genre = "Action",
                                            Rating = 5,
                                            ReleaseYear = 2014,
                                            Title = "Movie4",
                                            Cast = new[] {"A3", "A4"}
                                        }
                                };
            return allMovies;
        }

        private void CheckAllPredefined(List<Movie> movies)
        {
            Assert.AreEqual(4, movies.Count);
            Assert.AreEqual(1, movies[0].Id);
            Assert.AreEqual(2, movies[1].Id);
            Assert.AreEqual(3, movies[2].Id);
            Assert.AreEqual(4, movies[3].Id);
        }

        private void CheckPropertyValueInList<T>(string propertyName, T[] values, List<Movie> movie)
        {
            PropertyInfo propertyDescriptor = typeof (Movie).GetProperty(propertyName);
            Assert.AreEqual(values[0], propertyDescriptor.GetValue(movie[0], new object[] {}));
            Assert.AreEqual(values[1], propertyDescriptor.GetValue(movie[1], new object[] {}));
            Assert.AreEqual(values[2], propertyDescriptor.GetValue(movie[2], new object[] {}));
            Assert.AreEqual(values[3], propertyDescriptor.GetValue(movie[3], new object[] {}));
        }

        [TestMethod]
        public void GetAllMoviesReturnsAllMoviesFromCache()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckAllPredefined(_moviesDataAccess.GetAllMovies().ToList());
        }

        [TestMethod]
        public void AddMovieAddsNewMovieToCache()
        {
            _moviesDataAccess.AddMovie(new Movie
                                           {
                                               Id = 5,
                                               Classification = "M",
                                               Genre = "Action",
                                               Rating = 1,
                                               ReleaseYear = 2014,
                                               Title = "Movie5",
                                               Cast = new[] {"A3", "A4"}
                                           });
            _movieCacheMock.Verify(c => c.AddMovie(It.IsAny<Movie>()), Times.Once());
        }

        [TestMethod]
        public void UpdateMovieUpdatessNewMovieToCache()
        {
            _moviesDataAccess.UpdateMovie(new Movie
                                              {
                                                  Id = 5,
                                                  Classification = "M",
                                                  Genre = "Action",
                                                  Rating = 1,
                                                  ReleaseYear = 2014,
                                                  Title = "Movie5",
                                                  Cast = new[] {"A3", "A4"}
                                              });
            _movieCacheMock.Verify(c => c.UpdateMovie(It.IsAny<Movie>()), Times.Once());
        }

        [TestMethod]
        public void GetMoiveByIdReturnsMovieFromCache()
        {
            _movieCacheMock.Setup(c => c.GetMovieById("1"))
                           .Returns(new Movie
                                        {
                                            Id = 5,
                                            Classification = "M",
                                            Genre = "Action",
                                            Rating = 1,
                                            ReleaseYear = 2014,
                                            Title = "Movie5",
                                            Cast = new[] {"A3", "A4"}
                                        });
            Movie result = _moviesDataAccess.GetMovieById("1");
            Assert.AreEqual(5, result.Id);
            Assert.AreEqual("M", result.Classification);
            Assert.AreEqual("Action", result.Genre);
            Assert.AreEqual(1, result.Rating);
            Assert.AreEqual(2014, result.ReleaseYear);
            Assert.AreEqual("Movie5", result.Title);
        }

        [TestMethod]
        public void SearchNullOrEmptyFieldReturnAll()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckAllPredefined(_moviesDataAccess.SearchMovies(null, "a").ToList());
            CheckAllPredefined(_moviesDataAccess.SearchMovies(string.Empty, "a").ToList());
        }

        [TestMethod]
        public void SearchNullOrEmptyExpressionReturnAll()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckAllPredefined(_moviesDataAccess.SearchMovies("a", null).ToList());
            CheckAllPredefined(_moviesDataAccess.SearchMovies("a", string.Empty).ToList());
        }

        [TestMethod]
        public void SearchByIntReturnsExectValue()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            Movie searchResult = _moviesDataAccess.SearchMovies("Id", "1").FirstOrDefault();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(1, searchResult.Id);
        }

        [TestMethod]
        public void SearchByStringReturnsContaiedValue()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            Movie searchResult = _moviesDataAccess.SearchMovies("Genre", "cart").FirstOrDefault();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(3, searchResult.Id);
            Assert.AreEqual("Cartoon", searchResult.Genre);
        }

        [TestMethod]
        public void SearchByCastReturnsMoviesWithActor()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            Movie searchResult = _moviesDataAccess.SearchMovies("Cast", "a1").FirstOrDefault();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(1, searchResult.Id);
            Assert.AreEqual("A1", searchResult.Cast[0]);
            Assert.AreEqual("A2", searchResult.Cast[1]);
        }

        [TestMethod]
        public void GetSortedByIdReturnsSortedLits()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckPropertyValueInList("Id", new[] {1, 2, 3, 4},
                                     _moviesDataAccess.GetAllMovies("Id", SortDirection.Asc).ToList());
            CheckPropertyValueInList("Id", new[] {4, 3, 2, 1},
                                     _moviesDataAccess.GetAllMovies("Id", SortDirection.Desc).ToList());
        }

        [TestMethod]
        public void GetSortedByTitleReturnsSortedLits()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckPropertyValueInList("Title", new[] {"Movie1", "Movie2", "Movie3", "Movie4"},
                                     _moviesDataAccess.GetAllMovies("Title", SortDirection.Asc).ToList());
            CheckPropertyValueInList("Title", new[] {"Movie4", "Movie3", "Movie2", "Movie1"},
                                     _moviesDataAccess.GetAllMovies("Title", SortDirection.Desc).ToList());
        }

        [TestMethod]
        public void GetSortedByGenreReturnsSortedLits()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckPropertyValueInList("Genre", new[] {"Action", "Action", "Cartoon", "Comedy"},
                                     _moviesDataAccess.GetAllMovies("Genre", SortDirection.Asc).ToList());
            CheckPropertyValueInList("Genre", new[] {"Comedy", "Cartoon", "Action", "Action"},
                                     _moviesDataAccess.GetAllMovies("Genre", SortDirection.Desc).ToList());
        }

        [TestMethod]
        public void GetSortedByClassificationReturnsSortedLits()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckPropertyValueInList("Classification", new[] {"M", "M", "M1", "M2"},
                                     _moviesDataAccess.GetAllMovies("Classification", SortDirection.Asc).ToList());
            CheckPropertyValueInList("Classification", new[] {"M2", "M1", "M", "M"},
                                     _moviesDataAccess.GetAllMovies("Classification", SortDirection.Desc).ToList());
        }

        [TestMethod]
        public void GetSortedByRatingReturnsSortedLits()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckPropertyValueInList("Rating", new[] {1, 3, 3, 5},
                                     _moviesDataAccess.GetAllMovies("Rating", SortDirection.Asc).ToList());
            CheckPropertyValueInList("Rating", new[] {5, 3, 3, 1},
                                     _moviesDataAccess.GetAllMovies("Rating", SortDirection.Desc).ToList());
        }

        [TestMethod]
        public void GetSortedByReleaseYearReturnsSortedLits()
        {
            _movieCacheMock.Setup(c => c.GetMovies()).Returns(GetPredefinedMovies());
            CheckPropertyValueInList("ReleaseYear", new[] {2010, 2012, 2014, 2014},
                                     _moviesDataAccess.GetAllMovies("ReleaseYear", SortDirection.Asc).ToList());
            CheckPropertyValueInList("ReleaseYear", new[] {2014, 2014, 2012, 2010},
                                     _moviesDataAccess.GetAllMovies("ReleaseYear", SortDirection.Desc).ToList());
        }
    }
}