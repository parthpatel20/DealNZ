﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoPattern.Models.RepositoryFiles
{
    public interface _IRepositoryList<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetByID(int ID);

        void Insert(T Class);
        void InsertRange(IEnumerable<T> Classes);

        void Delete(T Class);
        void DeleteRange(IEnumerable<T> Classes);

        //  void UpdateClass(T Class);
        void SaveChange();


    }
}