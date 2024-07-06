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
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(O => O.Status)
				.HasConversion(
				(OStatus)=>OStatus.ToString(),
				(OStatus)=>(OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)
				);

			builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

			builder.Property(order => order.SubTotal)
				.HasColumnType("decimal(18,2)");

			builder.HasOne(O => O.DeliveryMethod)
				.WithMany()
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(O => O.Items)
				.WithOne()
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
