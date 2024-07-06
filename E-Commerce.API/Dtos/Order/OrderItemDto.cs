﻿namespace E_Commerce.API.Dtos.Order
{
	public class OrderItemDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}