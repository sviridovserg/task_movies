using System;
using System.Collections.Generic;
using Movies.DataContracts;

namespace Movies.Models
{
    public class MovieComparer: IComparer<Movie>
    {
        private string _field;
        private SortDirection _direction;

        public MovieComparer(string field, SortDirection direction)
        {
            _field = field;
            _direction = direction;
        }

        public int Compare(Movie x, Movie y)
        {
            IComparable xValue = (typeof(Movie)).GetProperty(_field).GetValue(x, new object[] { }) as IComparable;
            IComparable yValue = (typeof(Movie)).GetProperty(_field).GetValue(y, new object[] { }) as IComparable;

            if (_direction == SortDirection.Asc)
            {
                return xValue.CompareTo(yValue);
            }
            else
            {
                return yValue.CompareTo(xValue);
            }
        }
    }
}