using Online_Book_Store_Application.Models.Models;

namespace Online_Book_Store_Application.Repository.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company entity);
}