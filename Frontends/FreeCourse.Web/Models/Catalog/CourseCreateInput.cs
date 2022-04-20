using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FreeCourse.Web.Models.Catalog
{
	public class CourseCreateInput
	{

        [Display(Name="Course Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string UserID { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Course Category")]
        public string CategoryId { get; set; }

        [Display(Name = "Course Picture")]
        public IFormFile PhotoFormFile { get; set; }
    }
}

