using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
	public class FeatureViewModel
	{
        [Display(Name ="Course Duration")]
        [Required]
        public int Duration { get; set; }
    }
}

