using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(int id);

		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter=null);

        IEnumerable<T> Where(Expression<Func<T, bool>> filter=null);

		Task AddAsync(T entity);

		void Remove(T entity);

		T Update(T entity);
	}
}
