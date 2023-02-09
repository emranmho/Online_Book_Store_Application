using Online_Book_Store_Application.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);
		void UpdateStatus(int id,string orderStatus,string? paymentStatus=null);
		void UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId);
	}
}
