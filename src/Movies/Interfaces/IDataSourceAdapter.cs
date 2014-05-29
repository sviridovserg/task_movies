﻿using System.Collections.Generic;
using Movies.DataContracts;

namespace Movies.Interfaces
{
    public interface IDataSourceAdapter
    {
        int Create(Movie movie);
        List<Movie> GetAllData();
        Movie GetDataById(int id);
        void Update(Movie movie);
    }
}