using System;
using System.Collections.Generic;

namespace FreeCourse.Web.Models.Orders
{
	public class OrderCreateInput
	{
		public string BuyerId { get; set; }

		public List<OrderItemCreateInput> OrderItems { get; set; }

		public AddressCreateInput Address { get; set; }
	}
}

