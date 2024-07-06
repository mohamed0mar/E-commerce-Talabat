using E_Commerce.Core;
using E_Commerce.Core.Entities.Order_Aggregate;
using E_Commerce.Core.Entities.Product;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Core.Specifications.Order_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Order_Service
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		public OrderService(
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork,
			IPaymentService paymentService
			)
		{
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}
		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, OrderAddress shippingAddress)
		{
			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo

			var orderItems = new List<OrderItem>();
			if (basket?.Items?.Count > 0)
			{
				var productRepo = _unitOfWork.Repository<Product>();
				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetAsync(item.Id);
					var productItemOrder = new ProductItemOrderd(item.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal
			var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

			// 4. Get Delivery Method From DeliveryMethods Repo
			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			//Check if there are Order or not
			var orderRepo = _unitOfWork.Repository<Order>();
			var spec = new OrderWithPaymentIntentSpecifications(basket?.PaymentIntentId);

			var existingOrder = await orderRepo.GetWithSpecAsync(spec);
			if(existingOrder is not null)
			{
				orderRepo.Delete(existingOrder);
				await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			}


			// 5. Create Order
			var order = new Order(
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				deliveryMethod: deliveryMethod,
				items: orderItems,
				subTotal: subTotal,
				paymentIntentId: basket?.PaymentIntentId ?? ""
				);
			orderRepo.Add(order);
			// 6. Save To Database [TODO]
			int result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null;
			return order;
		}


		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			var orderRepo = _unitOfWork.Repository<Order>();
			var spec = new OrderSpecifications(buyerEmail);
			var orders = orderRepo.GetAllWithSpecAsync(spec);
			return orders;
		}

		public Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			var orderRepo = _unitOfWork.Repository<Order>();

			var spec = new OrderSpecifications(buyerEmail, orderId);

			var order = orderRepo.GetWithSpecAsync(spec);
			return order;
		}


		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
			=> await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
	}
}
