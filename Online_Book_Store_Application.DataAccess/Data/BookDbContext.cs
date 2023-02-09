using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.Models.Models;

namespace Online_Book_Store_Application.DataAccess;
    
    public class BookDbContext : IdentityDbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) 
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
	    public DbSet<OrderHeader> OrderHeaders { get; set; }
	    public DbSet<OrderDetail> OrderDetails { get; set; }
}

