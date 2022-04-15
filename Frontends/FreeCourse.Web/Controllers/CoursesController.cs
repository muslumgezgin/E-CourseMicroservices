using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;

        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }



        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var userId = _sharedIdentityService.GetUserId; 
            return View(await _catalogService.GetAllCourseByUserIdAsync(userId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");


            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }

            courseCreateInput.UserID = _sharedIdentityService.GetUserId;

            await _catalogService.CreateCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseId(id);
            var categories = await _catalogService.GetAllCategoryAsync();

            if(course == null)
            {
                RedirectToAction(nameof(Index));
            }

            ViewBag.categoryList = new SelectList(categories, "Id", "Name",course.Id);
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description= course.Description,
                Feature = course.Feature,
                CategoryId = course.CategoryId,
                UserID = course.UserID,
                Picture = course.Picture

            };

            return View(courseUpdateInput);

        }
    }
}

