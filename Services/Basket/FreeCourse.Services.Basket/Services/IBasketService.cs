using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services
{
	public interface IBasketService
	{

		Task<Response<BasketDto>> GetBasket(string userId);

		Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
		Task SaveOrUpdateAllKeys(CourseNameChangedEvent changedCourse);
		Task<Response<bool>> Delete(string userId);


	}
}

