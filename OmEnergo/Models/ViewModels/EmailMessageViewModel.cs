using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models.ViewModels
{
	public class EmailMessageViewModel
	{
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Пожалуйста, заполните поле")]
		public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Пожалуйста, заполните поле")]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Неправильный email")]
		public string Email { get; set; }

		[Display(Name = "Номер телефона")]
		public string PhoneNumber { get; set; }

        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Пожалуйста, заполните поле")]
		public string Text { get; set; }
	}
}
