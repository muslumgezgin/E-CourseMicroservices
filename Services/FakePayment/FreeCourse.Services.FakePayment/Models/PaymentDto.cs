using System;
namespace FreeCourse.Services.FakePayment.Models
{
	public class PaymentDto
	{
		public string CardName { get; set; }

		public string CardNumber { get; set; }

		public string Expiration { get; set; }

		public string CVC { get; set; }

		public decimal TotalPrice { get; set; }

        public OrderDto Order { get; set; }
    }
}

