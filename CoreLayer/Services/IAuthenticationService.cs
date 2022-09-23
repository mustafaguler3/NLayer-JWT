using CoreLayer.Dtos;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
	public interface IAuthenticationService
	{
		Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
		
		Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
		Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

		Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
	}
}
