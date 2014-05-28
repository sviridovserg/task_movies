using System;
using System.Collections.Generic;
using System.Reflection;
using Movies.DataContracts;

namespace Movies.Models
{
    public sealed class MovieComparer : IComparer<Movie>
    {
        private readonly SortDirection _direction;
        private readonly PropertyInfo _propertyToCompare;

        public MovieComparer(string field, SortDirection direction)
        {
            _propertyToCompare = (typeof(Movie)).GetProperty(field);
#warning _propertyToCompare can be null
            _direction = direction;
        }

        public int Compare(Movie x, Movie y)
        {
            var xValue = GetComparablePropertyValue(x);
            var yValue = GetComparablePropertyValue(y);

            return _direction == SortDirection.Asc ? xValue.CompareTo(yValue) : yValue.CompareTo(xValue);
        }

        private IComparable GetComparablePropertyValue(Movie obj)
        {
            return _propertyToCompare.GetValue(obj, new object[] { }) as IComparable;
        }
    }
}