using AutoMapper;
using CoreLayer.Dtos;
using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Mappings
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<User, UserDto>().ReverseMap();
		}
	}
}
