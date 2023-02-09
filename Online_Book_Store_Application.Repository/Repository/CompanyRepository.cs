using Online_Book_Store_Application.DataAccess;
using Online_Book_Store_Application.Models.Models;
using Online_Book_Store_Application.Repository.IRepository;

namespace Online_Book_Store_Application.Repository.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private BookDbContext _context;
    public CompanyRepository(BookDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Company entity)
    {
        _context.Companies.Update(entity);
    }
}