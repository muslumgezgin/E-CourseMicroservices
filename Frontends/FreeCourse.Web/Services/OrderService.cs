using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
	public class OrderService :IOrderService
	{
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(IPaymentService paymentService, HttpClient httpClient,
            IBasketService basketService,ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CrateOrder(CheckOutInfoInput checkOutInfo)
        {
            var basket = await _basketService.Get();
            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkOutInfo.CardName,
                CardNumber = checkOutInfo.CardNumber,
                CVC = checkOutInfo.CVV,
                Expiration = checkOutInfo.Expiration,
                TotalPrice = basket.TotalPrice

            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if(!responsePayment)
            {
                return new OrderCreatedViewModel()
                {
                    Error="Payment did  not recive",
                    IsSuccesfull = false
                };
            }

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkOutInfo.Province,
                    District=checkOutInfo.District,
                    Street = checkOutInfo.Street,
                    Line =checkOutInfo.Line,
                    ZipCode = checkOutInfo.ZipCode
                },

            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput()
                {
                    Price=x.GetCurrentPrice,
                    ProductId= x.CourseId,
                    PictureUrl = "",
                    ProductName =x.CourseName
                };

                orderCreateInput.OrderItems.Add(orderItem);

            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders",orderCreateInput);

            if(!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel()
                {
                    Error = "fail to create order",
                    IsSuccesfull = false
                };

            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            orderCreatedViewModel.Data.IsSuccesfull = true;

            await _basketService.Delete();
            return orderCreatedViewModel.Data;

        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspenOrder(CheckOutInfoInput checkOutInfo)
        {
            var basket = await _basketService.Get();

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkOutInfo.Province,
                    District = checkOutInfo.District,
                    Street = checkOutInfo.Street,
                    Line = checkOutInfo.Line,
                    ZipCode = checkOutInfo.ZipCode
                },

            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput()
                {
                    Price = x.GetCurrentPrice,
                    ProductId = x.CourseId,
                    PictureUrl = "",
                    ProductName = x.CourseName
                };

                orderCreateInput.OrderItems.Add(orderItem);

            });


            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkOutInfo.CardName,
                CardNumber = checkOutInfo.CardNumber,
                CVC = checkOutInfo.CVV,
                Expiration = checkOutInfo.Expiration,
                TotalPrice = basket.TotalPrice,
                Order =orderCreateInput

            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderSuspendViewModel()
                {
                    Error = "Payment did  not recive",
                    IsSuccesfull = false
                };
            }

            await _basketService.Delete();

            return new OrderSuspendViewModel() { IsSuccesfull = true };

        }

    }
}

