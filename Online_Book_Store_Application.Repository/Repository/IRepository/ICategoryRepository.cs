using Online_Book_Store_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category entity);

        Task Save();
    }
}
