using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FreeCourse.Web.Models.Catalog
{
	public class CourseUpdateInput
	{
        public string Id { get; set; }

        [Display(Name = "Course Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string UserID { get; set; }

        [Required]
        public decimal Price { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Course Picture")]
        public IFormFile PhotoFormFile { get; set; }
    }
}

