using API.Configuration;
using CoreLayer.Dtos;
using CoreLayer.Entities;
using CoreLayer.Repositories;
using CoreLayer.Services;
using CoreLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly List<Client> _clients;
		private readonly ITokenService _tokenService;
		private readonly UserManager<User> _userManager;
		private readonly IUnitOfWork _uow;
		private readonly IGenericRepository<UserRefreshToken> _userRefreshRepository;

		public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<User> userManager, IUnitOfWork uow, IGenericRepository<UserRefreshToken> userRefreshRepository)
		{
			_clients = clients.Value;
			_tokenService = tokenService;
			_userManager = userManager;
			_uow = uow;
            _userRefreshRepository = userRefreshRepository;
		}

		public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
		{
			if (loginDto == null)
			{
				throw new ArgumentNullException(nameof(loginDto));
			}

			var user = await _userManager.FindByEmailAsync(loginDto.Email);

			if (user == null)
			{
				return Response<TokenDto>.Fail("email or password is wrong", 404, true);
			}

			if (!await _userManager.CheckPasswordAsync(user,loginDto.Password))
			{
                return Response<TokenDto>.Fail("email or password is wrong", 400, true);
            }

			var token = _tokenService.CreateToken(user);

			var userRefreshToken = _userRefreshRepository.Where(i => i.UserId == user.Id).FirstOrDefault();

			if (userRefreshToken == null)
			{
				await _userRefreshRepository.AddAsync(new UserRefreshToken
				{
					UserId = user.Id,
					RefreshToken = token.RefreshToken,
					Expiration = token.RefreshTokenExpiration
				});
			}else
			{
				userRefreshToken.RefreshToken = token.RefreshToken;
				userRefreshToken.Expiration = token.RefreshTokenExpiration;
			}

			await _uow.CommitAsync();

			return Response<TokenDto>.Success(token,200);
		}

		public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
		{
			var client = _clients.SingleOrDefault(i => i.Id == clientLoginDto.ClientId && i.Secret == clientLoginDto.ClientSecret);

			if (client == null)
			{
				return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
			}
			var token = _tokenService.CreateTokenByClient(client);

			return Response<ClientTokenDto>.Success(token,200);
		}

		public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
		{
			var existingToken = _userRefreshRepository.Where(i => i.RefreshToken == refreshToken).FirstOrDefault();

			if (existingToken == null)
			{
				return Response<TokenDto>.Fail("Refreshtoken not found", 404, true);
			}

			var user = await _userManager.FindByIdAsync(existingToken.UserId);

            if (user == null)
            {
                return Response<TokenDto>.Fail("userId not found", 404, true);
            }

			var tokenDto = _tokenService.CreateToken(user);

			existingToken.RefreshToken = tokenDto.RefreshToken;
			existingToken.Expiration = tokenDto.RefreshTokenExpiration;

			await _uow.CommitAsync();

			return Response<TokenDto>.Success(tokenDto,200);
        }

		public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
		{
			var existingRefreshToken = _userRefreshRepository.Where(i => i.RefreshToken == refreshToken).FirstOrDefault();

            if (existingRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refreshtoken not found", 404, true);
            }

			_userRefreshRepository.Remove(existingRefreshToken);
			await _uow.CommitAsync();

			return Response<NoDataDto>.Success(200);
        }
	}
}
