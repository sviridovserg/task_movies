using System.Runtime.Serialization;
using MoviesLibrary;

namespace Movies.DataContracts
{
    [DataContract]
    public class Movie
    {
        public Movie()
        {
        }

        public Movie(MovieData data)
        {
            Id = data.MovieId;
            Title = data.Title;
            Rating = data.Rating;
            ReleaseYear = data.ReleaseDate;
            Classification = data.Classification;
            Genre = data.Genre;
            if (data.Cast != null)
            {
                Cast = new string[data.Cast.Length];
                data.Cast.CopyTo(Cast, 0);
            }
            else
            {
                Cast = new string[0];
            }
        }

        [DataMember]
        public string CacheId { get; set; }

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

        public bool IsNew()
        {
            return Id == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Movie);
        }

        public bool Equals(Movie obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Id == obj.Id && Title == obj.Title && ReleaseYear == obj.ReleaseYear && Genre == obj.Genre &&
                   Rating == obj.Rating &&
                   Classification == obj.Classification;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Title.GetHashCode() ^ ReleaseYear.GetHashCode() ^ Genre.GetHashCode() ^
                   Rating.GetHashCode() ^
                   Classification.GetHashCode();
        }

        public static implicit operator Movie(MovieData data)
        {
            return new Movie(data);
        }

        public static explicit operator MovieData(Movie movie)
        {
            var data = new MovieData();
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