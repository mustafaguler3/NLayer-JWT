﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Dtos
{
	public class CreateUserDto
	{
		public string UserName { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string City { get; set; }

	}
}
