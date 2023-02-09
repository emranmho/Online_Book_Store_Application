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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private BookDbContext _context;
        public ShoppingCartRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecrementUpdate(ShoppingCart entity, int count)
        {
            entity.Count -= count;
            return entity.Count;
        }

        public int IncrementUpdate(ShoppingCart entity, int count)
        {
            entity.Count += count;
            return entity.Count;
        }
    }
}
