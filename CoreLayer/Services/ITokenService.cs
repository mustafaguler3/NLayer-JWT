using CoreLayer.Configuration;
using CoreLayer.Dtos;
using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
	public interface ITokenService
	{
		TokenDto CreateToken(User user);

		ClientTokenDto CreateTokenByClient(Client client);
	}
}
