using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entities.Order_Aggregate
{
	public class Order : BaseEntity
	{
		private Order()
		{
		}
		public Order(string buyerEmail, OrderAddress shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
		{
			BuyerEmail = buyerEmail;
			ShippingAddress = shippingAddress;
			DeliveryMethod = deliveryMethod;
			Items = items;
			SubTotal = subTotal;
			PaymentIntentId = paymentIntentId;
		}

		public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public OrderAddress ShippingAddress { get; set; } = null!;
		public virtual DeliveryMethod? DeliveryMethod { get; set; } = null!; //Navigatial Property One
		public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
		public decimal SubTotal
		{
			get; set;
		}
		public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
		public string PaymentIntentId { get; set; }
	}
}
