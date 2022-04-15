using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FreeCourse.Web.Models.Catalog
{
	public class CourseCreateInput
	{

        [Display(Name="Course Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string UserID { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Course Category")]
        [Required]
        public string CategoryId { get; set; }

        [Display(Name = "Course Picture")]
        public IFormFile PhotoFormFile { get; set; }
    }
}

