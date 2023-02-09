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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private BookDbContext _context;
        public ApplicationUserRepository(BookDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
