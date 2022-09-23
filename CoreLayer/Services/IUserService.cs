using CoreLayer.Dtos;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
	public interface IUserService
	{
		Task<Response<UserDto>> CreateUserAsync(CreateUserDto userDto);

        Task<Response<UserDto>> GetUserByNameAsync(string userName);
    }
}
