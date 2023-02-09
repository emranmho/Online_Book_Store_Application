using Online_Book_Store_Application.Models;
using Online_Book_Store_Application.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncrementUpdate(ShoppingCart entity, int count);
        int DecrementUpdate(ShoppingCart entity, int count);
    }
}
