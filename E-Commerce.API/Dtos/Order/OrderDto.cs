using System.ComponentModel.DataAnnotations;

namespace E_Commerce.API.Dtos.Order
{
	public class OrderDto
	{
        [Required]
		public string BasketId { get; set; } = null!;
		[Required]
		public int DeliveryMethodId { get; set; } 
		public OrderAddressDto ShippingAddress { get; set; } = null!;
	}
}
