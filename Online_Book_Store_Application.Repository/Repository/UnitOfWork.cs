using Online_Book_Store_Application.DataAccess.Data;
using Online_Book_Store_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private BookDbContext _context;
        public UnitOfWork(BookDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            BookAuthor = new BookAuthorRepository(_context);
            Book = new BookRepository(_context);
            Company = new CompanyRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
			OrderDetail = new OrderDetailRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);

		}
        public ICategoryRepository Category { get; private set; }
        public IBookAuthorRepository BookAuthor { get; private set; }
        public IBookRepository Book { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

		public IOrderDetailRepository OrderDetail { get; private set; }

		public IOrderHeaderRepository OrderHeader { get; private set; }

		public void Save()
        {
            _context.SaveChanges();
        }
    }
}
