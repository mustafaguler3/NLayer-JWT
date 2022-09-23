using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
	public interface IService<T,TDto> 
        where T : class
        where TDto : class
	{
        Task<Response<TDto>> GetByIdAsync(int id);

        Task<Response<IEnumerable<TDto>>> GetAllAsync();

        Task<Response<IQueryable<TDto>>> Where(Expression<Func<T, bool>> filter);

        Task<Response<TDto>> AddAsync(T entity);

        Task<Response<NoDataDto>> Remove(int id);

        Task<Response<NoDataDto>> Update(T entity, int id);
    }
}
