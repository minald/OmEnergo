using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace OmEnergo.Infrastructure
{
    public class EmailSender
    {
		public IConfiguration Configuration { get; set; }

		public EmailSender(IConfiguration configuration) => Configuration = configuration;

		public void SendEmail(string name, string text, string email, string phoneNumber)
		{
			using (var client = new SmtpClient())
			{
			    client.EnableSsl = Configuration["Email:EnableSsl"] == "true";
				client.Host = Configuration["Email:Host"];
				client.Port = Int32.Parse(Configuration["Email:Port"]);
				client.Credentials = new NetworkCredential(Configuration["Email:From"], Configuration["Email:Password"]);
                MailMessage message = CreateMessage(name, text, email, phoneNumber);
                client.Send(message);
			}
		}

		private MailMessage CreateMessage(string name, string text, string email, string phoneNumber)
		{
		    var mailMessage = new MailMessage
			{
				From = new MailAddress(Configuration["Email:From"]),
				Subject = "Обратная связь",
				Body = $"Имя: {name}" + Environment.NewLine
                     + $"Телефон: {phoneNumber}" + Environment.NewLine
                     + $"Email: {email}" + Environment.NewLine 
                     + $"Сообщение: {text}"
			};
			mailMessage.To.Add(Configuration["Email:To"]);
			return mailMessage;
		}
	}
}
