using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Movies.DataContracts;
using Movies.Interfaces;
using Movies.Utils;

namespace MoviesUnitTests
{
    [TestClass]
    public class DataMergerTest
    {
        private Mock<IDataSourceAdapter> _dataAdapterMock;
        private IDataMerger _merger;
        private Mock<IMovieCache> _movieCacheMock;

        [TestInitialize]
        public void TestInit()
        {
            _dataAdapterMock = new Mock<IDataSourceAdapter>();
            _movieCacheMock = new Mock<IMovieCache>();
            _merger = new DataMerger(_movieCacheMock.Object, _dataAdapterMock.Object, new Mock<ILogger>().Object);
        }

        [TestMethod]
        public void MergeAddsMissedMovies()
        {
            var addedMovie1 = new Movie
                                  {
                                      CacheId = "1",
                                      Id = 0,
                                      Classification = "M2",
                                      Genre = "Cartoon",
                                      Rating = 3,
                                      ReleaseYear = 2012,
                                      Title = "Movie3",
                                      Cast = null
                                  };
            var addedMovie2 = new Movie
                                  {
                                      CacheId = "2",
                                      Id = 0,
                                      Classification = "M",
                                      Genre = "Action",
                                      Rating = 5,
                                      ReleaseYear = 2014,
                                      Title = "Movie4",
                                      Cast = new[] {"A3", "A4"}
                                  };
            _dataAdapterMock.Setup(d => d.GetAllMovies()).Returns(new List<Movie>
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
                                                                              }
                                                                      });
            _movieCacheMock.Setup(d => d.GetMovies()).Returns(new List<Movie>
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
                                                                      addedMovie1,
                                                                      addedMovie2
                                                                  });
            _merger.MergeCacheIntoDatsource();
            _dataAdapterMock.Verify(d => d.Create(addedMovie1), Times.Once());
            _dataAdapterMock.Verify(d => d.Create(addedMovie2), Times.Once());
        }

        [TestMethod]
        public void MergeUpdatesModifiedMovies()
        {
            var updatedMovie1 = new Movie
                                    {
                                        CacheId = "3",
                                        Id = 3,
                                        Classification = "M2",
                                        Genre = "Cartoon, Comedy",
                                        Rating = 3,
                                        ReleaseYear = 2012,
                                        Title = "Movie3",
                                        Cast = null
                                    };
            var updatedMovie2 = new Movie
                                    {
                                        CacheId = "4",
                                        Id = 4,
                                        Classification = "M",
                                        Genre = "Action, Cartoon",
                                        Rating = 5,
                                        ReleaseYear = 2014,
                                        Title = "Movie4",
                                        Cast = new[] {"A3", "A4"}
                                    };
            _dataAdapterMock.Setup(d => d.GetAllMovies()).Returns(new List<Movie>
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
                                                                      });
            _movieCacheMock.Setup(d => d.GetMovies()).Returns(new List<Movie>
                                                                  {
                                                                      new Movie
                                                                          {
                                                                              CacheId = "1",
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
                                                                              CacheId = "2",
                                                                              Id = 2,
                                                                              Classification = "M1",
                                                                              Genre = "Comedy",
                                                                              Rating = 3,
                                                                              ReleaseYear = 2010,
                                                                              Title = "Movie2",
                                                                              Cast = new string[] {}
                                                                          },
                                                                      updatedMovie1,
                                                                      updatedMovie2
                                                                  });
            _merger.MergeCacheIntoDatsource();
            _dataAdapterMock.Verify(d => d.Update(updatedMovie1), Times.Once());
            _dataAdapterMock.Verify(d => d.Update(updatedMovie2), Times.Once());
        }

        [TestMethod]
        public void MergerPushChangesToDataSource()
        {
            var addedMovie1 = new Movie
                                  {
                                      CacheId = "3",
                                      Id = 0,
                                      Classification = "M2",
                                      Genre = "Cartoon, Comedy",
                                      Rating = 3,
                                      ReleaseYear = 2012,
                                      Title = "Movie3",
                                      Cast = null
                                  };
            var updatedMovie1 = new Movie
                                    {
                                        CacheId = "4",
                                        Id = 4,
                                        Classification = "M",
                                        Genre = "Action, Cartoon",
                                        Rating = 5,
                                        ReleaseYear = 2014,
                                        Title = "Movie4",
                                        Cast = new[] {"A3", "A4"}
                                    };
            _dataAdapterMock.Setup(d => d.GetAllMovies()).Returns(new List<Movie>
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
                                                                                  Id = 4,
                                                                                  Classification = "M",
                                                                                  Genre = "Action",
                                                                                  Rating = 5,
                                                                                  ReleaseYear = 2014,
                                                                                  Title = "Movie4",
                                                                                  Cast = new[] {"A3", "A4"}
                                                                              }
                                                                      });
            _movieCacheMock.Setup(d => d.GetMovies()).Returns(new List<Movie>
                                                                  {
                                                                      new Movie
                                                                          {
                                                                              CacheId = "1",
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
                                                                              CacheId = "2",
                                                                              Id = 2,
                                                                              Classification = "M1",
                                                                              Genre = "Comedy",
                                                                              Rating = 3,
                                                                              ReleaseYear = 2010,
                                                                              Title = "Movie2",
                                                                              Cast = new string[] {}
                                                                          },
                                                                      updatedMovie1,
                                                                      addedMovie1
                                                                  });
            _merger.MergeCacheIntoDatsource();
            _dataAdapterMock.Verify(d => d.Update(updatedMovie1), Times.Once());
            _dataAdapterMock.Verify(d => d.Create(addedMovie1), Times.Once());
        }

        [TestMethod]
        public void NoChangesPushedIfCacheUnchanged()
        {
            _dataAdapterMock.Setup(d => d.GetAllMovies()).Returns(new List<Movie>
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
                                                                              }
                                                                      });
            _movieCacheMock.Setup(d => d.GetMovies()).Returns(new List<Movie>
                                                                  {
                                                                      new Movie
                                                                          {
                                                                              CacheId = "1",
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
                                                                              CacheId = "2",
                                                                              Id = 2,
                                                                              Classification = "M1",
                                                                              Genre = "Comedy",
                                                                              Rating = 3,
                                                                              ReleaseYear = 2010,
                                                                              Title = "Movie2",
                                                                              Cast = new string[] {}
                                                                          }
                                                                  });
            _merger.MergeCacheIntoDatsource();
            _dataAdapterMock.Verify(d => d.Update(It.IsAny<Movie>()), Times.Never());
            _dataAdapterMock.Verify(d => d.Create(It.IsAny<Movie>()), Times.Never());
        }
    }
}