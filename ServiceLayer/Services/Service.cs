using CoreLayer.Repositories;
using CoreLayer.Services;
using CoreLayer.UnitOfWork;
using ServiceLayer.Mappings;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
	public class Service<T, TDto> : IService<T, TDto>
		where T : class
		where TDto : class
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<T> _genericRepository;

		public Service(IGenericRepository<T> genericRepository, IUnitOfWork unitOfWork)
		{
			_genericRepository = genericRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Response<TDto>> AddAsync(T entity)
		{
			var newEntity = ObjectMapper.Mapper.Map<T>(entity);

			await _genericRepository.AddAsync(newEntity);
			await _unitOfWork.CommitAsync();

			var dto = ObjectMapper.Mapper.Map<TDto>(newEntity);

			return Response<TDto>.Success(dto, 200);
		}

		public Task<Response<IQueryable<TDto>>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Response<TDto>> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Response<NoDataDto>> Remove(T entity)
		{
			throw new NotImplementedException();
		}

		public Task<Response<NoDataDto>> Update(T entity)
		{
			throw new NotImplementedException();
		}

		public Task<Response<IQueryable<TDto>>> Where(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}
	}
}
