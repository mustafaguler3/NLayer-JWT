using CoreLayer.Repositories;
using CoreLayer.Services;
using CoreLayer.UnitOfWork;
using ServiceLayer.Mappings;
using Shared.Dtos;
using System;
using System.Collections;
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

		public async Task<Response<TDto>> AddAsync(TDto entity)
		{
			var newEntity = ObjectMapper.Mapper.Map<T>(entity);

			await _genericRepository.AddAsync(newEntity);

			await _unitOfWork.CommitAsync();

			var dto = ObjectMapper.Mapper.Map<TDto>(newEntity);

			return Response<TDto>.Success(dto, 200);
		}

		public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
		{
			var p = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

			return Response<IEnumerable<TDto>>.Success(p,200);
		}

		public async Task<Response<TDto>> GetByIdAsync(int id)
		{
			var p = await _genericRepository.GetByIdAsync(id);

			if (p == null)
			{
				return Response<TDto>.Fail("id not found", 404, true);
			}

			return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(p), 200);
		}

		public async Task<Response<NoDataDto>> Remove(int id)
		{
			var exist = await _genericRepository.GetByIdAsync(id);

			if (exist == null)
			{
				return Response<NoDataDto>.Fail("id not found", 404, true);
			}

			_genericRepository.Remove(exist);
			await _unitOfWork.CommitAsync();

			return Response<NoDataDto>.Success(ObjectMapper.Mapper.Map<NoDataDto>(exist), 200);
		}

		public async Task<Response<NoDataDto>> Update(TDto entity,int id)
		{
			var exist = await _genericRepository.GetByIdAsync(id);

			if (exist == null)
			{
                return Response<NoDataDto>.Fail("id not found", 404, true);
            }

			_genericRepository.Update(exist);
			await _unitOfWork.CommitAsync();

			return Response<NoDataDto>.Success(ObjectMapper.Mapper.Map<NoDataDto>(exist), 204);
		}

		/*public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> filter)
		{
			var p = _genericRepository.Where(filter);

			if (p == null)
			{
				return Response<IEnumerable<TDto>>.Fail("not found", 404, true);
			}

			return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await p.ToListAsync()));
		} */
	}
}
