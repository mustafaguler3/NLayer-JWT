﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configuration
{
	public class TokenOption
	{
		public List<string> Audience { get; set; }
		
		public string Issuer { get; set; }
		
		public int AccessTokenExpiration { get; set; }
		
		public int RefreshTokenExpiration { get; set; }
		
		public string SecurityKey { get; set; }
	}
}
