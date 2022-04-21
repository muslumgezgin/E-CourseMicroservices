using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interfaces
{
	public interface IOrderService
	{
		//not async 
		Task<OrderCreatedViewModel> CrateOrder(CheckOutInfoInput checkOutInfo);

		// async with rabbitmq
		Task<OrderSuspendViewModel> SuspenOrder(CheckOutInfoInput checkOutInfo);

		Task<List<OrderViewModel>> GetOrder();
	}
}

