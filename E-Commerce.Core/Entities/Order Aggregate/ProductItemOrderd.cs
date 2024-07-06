namespace E_Commerce.Core.Entities.Order_Aggregate
{
	public class ProductItemOrderd
	{
		private ProductItemOrderd()
		{
		}
		public ProductItemOrderd(int productId, string productName, string pictureUrl)
		{
			ProductId = productId;
			ProductName = productName;
			PictureUrl = pictureUrl;
		}

		public int ProductId
		{
			get; set;
		}
		public string ProductName { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
	}
}