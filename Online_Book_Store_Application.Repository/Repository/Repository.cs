using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.DataAccess;
using Online_Book_Store_Application.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private BookDbContext _context;
        internal DbSet<T> _dbSet;
        public Repository(BookDbContext context)
        {
            _context = context;
            this._dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IQueryable<T> query =  _dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

    }
}
