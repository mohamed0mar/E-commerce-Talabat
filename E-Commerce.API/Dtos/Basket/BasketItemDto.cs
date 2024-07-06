using System.ComponentModel.DataAnnotations;

namespace E_Commerce.API.Dtos.Basket
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string PictureUrl { get; set; } = null!;
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price Must Be More Than Zero.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must Be At Least One.")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        public string Brand { get; set; } = null!;
    }
}