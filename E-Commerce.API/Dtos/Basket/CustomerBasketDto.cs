using System.ComponentModel.DataAnnotations;

namespace E_Commerce.API.Dtos.Basket
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }	
        public string? ClientSecret { get; set; }
        [Required]
        public List<BasketItemDto> Items { get; set; } = null!;
    }
}
