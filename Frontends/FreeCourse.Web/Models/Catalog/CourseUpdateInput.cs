using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FreeCourse.Web.Models.Catalog
{
	public class CourseUpdateInput
	{
        public string Id { get; set; }

        [Display(Name = "Course Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string UserID { get; set; }

        public decimal Price { get; set; }

        public FeatureViewModel Feature { get; set; }

        public string CategoryId { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Course Picture")]
        public IFormFile PhotoFormFile { get; set; }
    }
}

