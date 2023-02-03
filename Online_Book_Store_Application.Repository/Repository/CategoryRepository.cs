using Online_Book_Store_Application.DataAccess;
using Online_Book_Store_Application.Models;
using Online_Book_Store_Application.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private BookDbContext _context;
        public CategoryRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async void Update(Category entity)
        {
            _context.Categories.Update(entity);
        }
    }
}
