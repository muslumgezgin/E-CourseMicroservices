using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreeCourse.Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var basket = await _basketService.Get();
            return View(basket);
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseId(courseId);
            var basketItem = new BasketItemViewModel
            {
                CourseName = course.Name,
                CourseId= course.Id,
                Price= course.Price,
            };

            await _basketService.AddBasketItem(basketItem);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId);

            return RedirectToAction(nameof(Index));
        }
    }
}

