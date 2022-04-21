using System;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Orders
{
	public class CheckOutInfoInput
	{
		public string Province { get; set; }

		public string District { get; set; }

		public string Street { get; set; }

		public string ZipCode { get; set; }

		public string Line { get; set; }

		[Display(Name ="Card name and surname")]
		public string CardName { get; set; }

		public string CardNumber { get; set; }

		[Display(Name ="expiration date")]
		public string Expiration { get; set; }

		[Display(Name ="CVC2/CVV number")]
		public string CVV { get; set; }

	}
}

