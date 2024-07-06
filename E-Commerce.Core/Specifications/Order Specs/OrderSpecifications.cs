using E_Commerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications.Order_Specs
{
	public class OrderSpecifications:BaseSpecifications<Order>
	{
        public OrderSpecifications(string buyerEmail)
            :base(O=>O.BuyerEmail==buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDesc(O => O.OrderDate);

		}

		public OrderSpecifications(string buyerEmail, int orderId)
		   : base(O => O.BuyerEmail == buyerEmail && O.Id==orderId)
		{
			Includes.Add(O => O.DeliveryMethod);
			Includes.Add(O => O.Items);

		}
	}
}
