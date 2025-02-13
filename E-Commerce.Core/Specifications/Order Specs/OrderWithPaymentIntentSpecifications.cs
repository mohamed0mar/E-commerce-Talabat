﻿using E_Commerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications.Order_Specs
{
	public class OrderWithPaymentIntentSpecifications:BaseSpecifications<Order>
	{
        public OrderWithPaymentIntentSpecifications(string? paymentIntentId)
            :base(O=>O.PaymentIntentId==paymentIntentId)
        {
            
        }
    }
}
