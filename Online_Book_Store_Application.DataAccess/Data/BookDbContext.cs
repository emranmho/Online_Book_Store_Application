using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.Models;

namespace Online_Book_Store_Application.DataAccess;
    
    public class BookDbContext :DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) 
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }

