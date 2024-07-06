using E_Commerce.Core.Entities.Order_Aggregate;

namespace E_Commerce.API.Dtos.Order
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } 
		public string Status { get; set; } 
		public OrderAddress ShippingAddress { get; set; } = null!;
		public string DeliveryMethod { get; set; } = null!;
		public decimal DeliveryMethodCost { get; set; } 
		public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
		public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
	}
}
