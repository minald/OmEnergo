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
		IConfiguration Configuration { get; }

		public EmailService()
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
		}

		public async Task SendEmail(string name, string text, string email, string phoneNumber)
		{
			using (var client = new SmtpClient())
			{
				client.EnableSsl = Configuration["EmailConfiguration:EmailEnableSsl"] == "true";
				client.Host = Configuration["EmailConfiguration:EmailHost"];
				client.Port = Int32.Parse(Configuration["EmailConfiguration:EmailPort"]);
				client.Credentials = new NetworkCredential(Configuration["EmailConfiguration:FromEmail"],
					Configuration["EmailConfiguration:EmailPassword"]);
				client.Send(CreateMessage(name, text, email, phoneNumber));
			}
		}

		private MailMessage CreateMessage(string name, string text, string email, string phoneNumber)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(Configuration["EmailConfiguration:FromEmail"]);
			mailMessage.To.Add(Configuration["EmailConfiguration:ToEmail"]);
			mailMessage.Body = "Имя: " + name + Environment.NewLine + "Телефон: " + phoneNumber + Environment.NewLine
				+ "Email: " + email + Environment.NewLine +"Сообщение: " + text;
			mailMessage.Subject = "Обратная связь";
			return mailMessage;
		}
	}
}
