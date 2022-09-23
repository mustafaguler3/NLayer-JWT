using CoreLayer.Dtos;
using CoreLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : CustomBaseController
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(CreateUserDto dto)
		{
			return ActionResultInstance(await _userService.CreateUserAsync(dto));
		}
	}
}
