using Online_Book_Store_Application.DataAccess.Data;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.Repository
{
    public class BookAuthorRepository : Repository<BookAuthor>, IBookAuthorRepository
    {
        private BookDbContext _context;
        public BookAuthorRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(BookAuthor entity)
        {
            _context.BookAuthors.Update(entity);
        }
    }
}
