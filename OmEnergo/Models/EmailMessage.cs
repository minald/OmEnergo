using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
{
    public class EmailMessage
    {
		public string Text { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }
    }
}
