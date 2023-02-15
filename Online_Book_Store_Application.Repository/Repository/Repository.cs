using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.DataAccess.Data;
using Online_Book_Store_Application.Repository.IRepository;
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
           // _context.Books.Include(x=>x.Category)
            this._dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            

            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }

            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter =null, string? includeProperties = null)
        {
            IQueryable<T> query =  _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }            
            if (includeProperties != null)
            {
                foreach(var property in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
                    
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
		public void RemoveRange(IEnumerable<T> entity)
		{
			_dbSet.RemoveRange(entity);
		}

	}
}
