using System.Runtime.Serialization;
using System.Linq;
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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Movie);
        }

        public bool Equals(Movie obj) 
        {
            if (obj == null) 
            {
                return false;
            }
            if ((this.Cast == null || obj.Cast == null) && this.Cast != obj.Cast) 
            {
                return false;
            }
            if ((this.Cast == obj.Cast) || this.Cast.Length != obj.Cast.Length) 
            {
                return false;
            }
            foreach (string c in this.Cast) 
            {
                if (!obj.Cast.Contains(c)) 
                {
                    return false;
                }
            }
            return this.Id == obj.Id && this.Title == obj.Title && this.ReleaseYear == obj.ReleaseYear && this.Genre == obj.Genre && this.Rating == obj.Rating &&
                this.Classification == obj.Classification;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.Title.GetHashCode() ^ this.ReleaseYear.GetHashCode() ^ this.Genre.GetHashCode() ^ this.Rating.GetHashCode() ^
                this.Classification.GetHashCode();
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