using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
{
    public class EmailMessage
    {
		[Required(ErrorMessage = "Не указан текст")]
		public string Text { get; set; }

		[Required(ErrorMessage = "Не указано имя")]
		public string Name { get; set; }

		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
		public string Email { get; set; }

		public string PhoneNumber { get; set; }
    }
}
