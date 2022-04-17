﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using FreeCourse.Web.Exceptions;

namespace FreeCourse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger,ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            var Courses = await _catalogService.GetAllCourseAsync();
            return View(Courses);
        }

        public async Task<IActionResult> Detail(string id)
        {
            return View(await _catalogService.GetByCourseId(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if(errorFeature != null  && errorFeature.Error is UnAuthorizedException)
            {
                return RedirectToAction(nameof(AuthController.LogOut), "Auth");
            }


            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

