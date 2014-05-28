using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MoviesLibrary;

namespace Movies.DataContracts
{
	[DataContract]
	public class Movie
	{
        public Movie() { }
        public Movie(MovieData data)
        {
            this.Id = data.MovieId;
            this.Title = data.Title;
            this.Rating = data.Rating;
            this.ReleaseYear = data.ReleaseDate;
            this.Classification = data.Classification;
            this.Genre = data.Genre;
            this.Cast = new string[data.Cast.Length];
            data.Cast.CopyTo(this.Cast, 0);
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int ReleaseYear { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public string Classification { get; set; }
        [DataMember]
        public string[] Cast { get; set; }

        static public implicit operator Movie(MovieData data) 
        {
            return new Movie(data);
        }

        static public explicit operator MovieData(Movie movie) 
        {
            MovieData data = new MovieData();
            data.MovieId = movie.Id;
            data.Title = movie.Title;
            data.Rating = movie.Rating;
            data.ReleaseDate = movie.ReleaseYear;
            data.Classification = movie.Classification;
            data.Genre = movie.Genre;
            data.Cast = new string[movie.Cast.Length];
            movie.Cast.CopyTo(data.Cast, 0);
            return data;
        }
	}
}