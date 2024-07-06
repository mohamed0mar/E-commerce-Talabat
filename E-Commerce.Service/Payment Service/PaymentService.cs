using E_Commerce.Core;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Entities.Order_Aggregate;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Core.Specifications.Order_Specs;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = E_Commerce.Core.Entities.Product.Product;

namespace E_Commerce.Service.Payment_Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBasketRepository _basketRepo;

		public PaymentService(
			IConfiguration configuration,
			IUnitOfWork unitOfWork,
			IBasketRepository basketRepo)
		{
			_configuration = configuration;
			_unitOfWork = unitOfWork;
			_basketRepo = basketRepo;
		}
		public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
		{
			//1. Get Secret Key
			StripeConfiguration.ApiKey = _configuration["StripeSetting:SecretKey"];

			//2. Get Basket from Basket Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);
			if (basket is null)
				return null;

			//3. Get Delivery Method Cost
			var shippingPrice = 0m;
			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
				shippingPrice = deliveryMethod.Cost;
				basket.ShippingPrice = shippingPrice;
			}

			//4. Check If The Price Is Correct Or Not
			if (basket?.Items?.Count > 0)
			{
				var productRepo = _unitOfWork.Repository<Product>();
				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetAsync(item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;
				}
			}


			//4. Create Or Update Payment Intent 

			PaymentIntent paymentIntent;
			PaymentIntentService paymentIntentService = new PaymentIntentService();

			if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Create Payment Intent
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long?)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string> { "card" }
				};
				paymentIntent = await paymentIntentService.CreateAsync(options); //Integration with stripe
				basket.PaymentIntentId= paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else  //Update Payment Intent
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long?)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
				};
				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
			}

			await _basketRepo.UpdateBasketAsync(basket);
			return (basket);
		}

		public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
		{
			var orderRepo = _unitOfWork.Repository<Order>();

			var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);

			var order = await orderRepo.GetWithSpecAsync(spec);

			if (order is null)
				return null;

			if (isPaid)
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaymentFailed;

			orderRepo.Update(order);

			await _unitOfWork.CompleteAsync();

			return (order);
		}
	}
}
