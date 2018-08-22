using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OmEnergo.Services
{
    public class EmailService
    {
		public static async Task SendEmail(string name, string text, string email, string phoneNumber)
		{
			IConfiguration Configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.Build();
			using (var client = new SmtpClient())
			{
				client.EnableSsl = Configuration["Email:EnableSsl"] == "true";
				client.Host = Configuration["Email:Host"];
				client.Port = Int32.Parse(Configuration["Email:Port"]);
				client.Credentials = new NetworkCredential(Configuration["Email:From"],
					Configuration["Email:Password"]);
				client.Send(CreateMessage(name, text, email, phoneNumber, Configuration));
			}
		}

		private static MailMessage CreateMessage(string name, string text, string email, string phoneNumber, IConfiguration Configuration)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(Configuration["Email:From"]);
			mailMessage.To.Add(Configuration["Email:To"]);
			mailMessage.Body = "Имя: " + name + Environment.NewLine + "Телефон: " + phoneNumber + Environment.NewLine
				+ "Email: " + email + Environment.NewLine +"Сообщение: " + text;
			mailMessage.Subject = "Обратная связь";
			return mailMessage;
		}
	}
}
