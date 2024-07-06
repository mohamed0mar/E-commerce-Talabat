using E_Commerce.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository._Data.Config.Order_Config
{
	internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.OwnsOne(orderItem => orderItem.Product, P => P.WithOwner());
			builder.Property(orderItem => orderItem.Price)
				.HasColumnType("decimal(18,2)");
		}
	}
}
