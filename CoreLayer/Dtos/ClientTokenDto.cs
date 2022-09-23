using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Dtos
{
	public class ClientTokenDto
	{
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
