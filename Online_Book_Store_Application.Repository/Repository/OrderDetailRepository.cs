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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private BookDbContext _context;
        public OrderDetailRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(OrderDetail entity)
        {
            _context.OrderDetails.Update(entity);
        }
    }
}
