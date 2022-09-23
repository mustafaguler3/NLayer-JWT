using CoreLayer.Configuration;
using CoreLayer.Dtos;
using CoreLayer.Entities;
using CoreLayer.Services;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Mappings;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;

		public UserService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto userDto)
		{
			var user = new User()
			{
				Email = userDto.Email,
				UserName = userDto.UserName,
				City = userDto.City
			};

			var result = await _userManager.CreateAsync(user, userDto.Password);

			if (!result.Succeeded)
			{
				var error = result.Errors.Select(i => i.Description).ToList();

				return Response<UserDto>.Fail(new ErrorDto(error, true), 400);
			}

			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user),200);
		}

		public async Task<Response<UserDto>> GetUserByNameAsync(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);

			if (user == null)
			{
				return Response<UserDto>.Fail("username not found",404,true);
			}

			return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
		}
	}
}


