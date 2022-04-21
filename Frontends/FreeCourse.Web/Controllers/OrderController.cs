using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();

            ViewBag.basket = basket;
            return View(new CheckOutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutInfoInput checkOutInfoInput)
        {
            var orderStatus = await _orderService.CrateOrder(checkOutInfoInput);

            if(!orderStatus.IsSuccesfull)
            {
                var basket = await _basketService.Get();

                ViewBag.basket = basket;

                ViewBag.error = orderStatus.Error;

                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;

            return View();
        }
       
    }
}

