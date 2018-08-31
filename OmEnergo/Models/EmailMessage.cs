using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class EmailMessage
    {
        [Required(ErrorMessage = "Пожалуйста, заполните поле")]
        [Display(Name = "Имя")]
		public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Неправильный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле")]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }
    }
}
