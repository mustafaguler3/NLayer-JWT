using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
	public class Response<T> where T : class
	{
		public T Data { get; set; }

		public int StatusCode { get; set; }

		public ErrorDto Error { get; set; }

		public bool IsSuccessful { get; set; }

		public static Response<T> Success(T data,int statusCode)
		{
			return new Response<T>
			{
				Data = data,
				StatusCode = statusCode,
				IsSuccessful = true
			};
		}

        public static Response<T> Success(int statusCode)
        {
            return new Response<T>
            {
                Data = default,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static Response<T> Fail(ErrorDto error, int statusCode)
        {
            return new Response<T>
            {
                Error = error,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string message, int statusCode,bool isShow)
        {
            var errorDto = new ErrorDto(message, isShow);
            return new Response<T>
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

		public static Response<IEnumerable<TDto>> Success<TDto>(IEnumerable<TDto> enumerable) where TDto : class
		{
			throw new NotImplementedException();
		}
	}
}
