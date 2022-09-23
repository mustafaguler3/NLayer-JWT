using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasKey(i => i.Id);
			builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
			builder.Property(i => i.Stock).IsRequired();
			//builder.Property(i => i.Price).HasColumnType("decimal(9,2)");
		}
	}
}
