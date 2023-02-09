﻿using Online_Book_Store_Application.Models;
using Online_Book_Store_Application.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.IRepository
{
    public interface IBookAuthorRepository : IRepository<BookAuthor>
    {
        void Update(BookAuthor entity);
    }
}
