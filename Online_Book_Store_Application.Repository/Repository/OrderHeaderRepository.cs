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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private BookDbContext _context;
        public OrderHeaderRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(OrderHeader entity)
        {
            _context.OrderHeaders.Update(entity);
        }

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus= orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.PaymentStatus= paymentStatus;
                }
            }
		}

		public void UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId)
		{
			var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
			
            orderFromDb.PaymentDate=DateTime.Now;
            orderFromDb.SessionId = SessionId;
            orderFromDb.PaymentIntentId = PaymentIntentId;
		}
	}
}
