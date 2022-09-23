using CoreLayer.Repositories;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T>
		where T : class
	{
		private readonly VtContext _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(VtContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter=null)
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public T Update(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;

			return entity;
		}

		public IEnumerable<T> Where(Expression<Func<T, bool>> filter=null)
		{
			return _dbSet.Where(filter);
		}
	}
}
