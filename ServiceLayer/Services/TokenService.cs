using CoreLayer.Configuration;
using CoreLayer.Dtos;
using CoreLayer.Entities;
using CoreLayer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ServiceLayer.Services
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<User> _userManager;
		private readonly TokenOption _tokenOption;

		public TokenService(UserManager<User> userManager, IOptions<TokenOption> tokenOption)
		{
			_userManager = userManager;
			_tokenOption = tokenOption.Value;
		}

		private string CreateRefreshToken()
		{
			var numberByte = new Byte[32];

			using var rnd = RandomNumberGenerator.Create();

			rnd.GetBytes(numberByte);

			return Convert.ToBase64String(numberByte);
        }

		private IEnumerable<Claim> GetClaim(User user,List<string> audience)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim(ClaimTypes.Name,user.UserName),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
			};
			claims.AddRange(audience.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

			return claims;
		}

        private IEnumerable<Claim> GetClaimsByClient(Client client)
		{
			var claims = new List<Claim>();

			claims.AddRange(client.Audiences.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
			new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString());

			return claims;

        }


        public TokenDto CreateToken(User user)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
			var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);

			var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				issuer:_tokenOption.Issuer,
				expires:accessTokenExpiration,
				notBefore:DateTime.Now,
				claims:GetClaim(user,_tokenOption.Audience),
				signingCredentials:signingCredentials
				);

			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwtSecurityToken);

			var tokenDto = new TokenDto
			{
				AccessToken = token,
				RefreshToken = CreateRefreshToken(),
				AccessTokenExpiration = accessTokenExpiration,
				RefreshTokenExpiration = refreshTokenExpiration
			};

			return tokenDto;
		}

		public ClientTokenDto CreateTokenByClient(Client client)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
			var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

			var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				issuer: _tokenOption.Issuer,
				expires: accessTokenExpiration,
				notBefore: DateTime.Now,
				claims: GetClaimsByClient(client),
				signingCredentials:signingCredentials);

			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwtSecurityToken);

			var tokenDto = new ClientTokenDto
			{
				AccessToken = token,
				AccessTokenExpiration = accessTokenExpiration
			};

			return tokenDto;
		}
	}
}
