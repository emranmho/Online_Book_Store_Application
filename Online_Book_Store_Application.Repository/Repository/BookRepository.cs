using Online_Book_Store_Application.DataAccess;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private BookDbContext _context;
        public BookRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Book entity)
        {
            var objFromDb = _context.Books.FirstOrDefault(x => x.Id == entity.Id);
            if(objFromDb != null)
            {
                objFromDb.Title= entity.Title;
                objFromDb.Description= entity.Description;
                objFromDb.Price= entity.Price;
                objFromDb.DiscountPercent= entity.DiscountPercent;
                objFromDb.PriceAfterDiscount= entity.PriceAfterDiscount;
                objFromDb.CategoryId= entity.CategoryId;
                objFromDb.BookAuthorId= entity.BookAuthorId;
                objFromDb.IsAvailable= entity.IsAvailable;
                if(entity.ImageUrl != null)
                {
                    objFromDb.ImageUrl= entity.ImageUrl;
                }
            }
        }
    }
}
