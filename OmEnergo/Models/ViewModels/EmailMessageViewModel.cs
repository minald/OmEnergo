using OmEnergo.Resources;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models.ViewModels
{
	public class EmailMessageViewModel
	{
		[Display(Name = "Name", ResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = "PleaseFillInTheField", ErrorMessageResourceType = typeof(SharedResource))]
		public string Name { get; set; }

		[Display(Name = "Email")]
		[Required(ErrorMessageResourceName = "PleaseFillInTheField", ErrorMessageResourceType = typeof(SharedResource))]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
			ErrorMessageResourceName = "WrongEmail", ErrorMessageResourceType = typeof(SharedResource))]
		public string Email { get; set; }

		[Display(Name = "PhoneNumber", ResourceType = typeof(SharedResource))]
		public string PhoneNumber { get; set; }

		[Display(Name = "Message", ResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = "PleaseFillInTheField", ErrorMessageResourceType = typeof(SharedResource))]
		public string Text { get; set; }
	}
}
