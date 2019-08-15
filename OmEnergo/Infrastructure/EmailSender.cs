using OmEnergo.Infrastructure.Database;
using System;
using System.Net;
using System.Net.Mail;

namespace OmEnergo.Infrastructure
{
	public class EmailSender
	{
		private readonly string Host;
		private readonly int Port;
		private readonly bool EnableSsl;
		private readonly string SenderEmailAddress;
		private readonly string SenderPassword;

		public EmailSender(Repository repository)
		{
			Host = repository.GetConfigValue("Email_Host");
			Port = Convert.ToInt32(repository.GetConfigValue("Email_Port"));
			EnableSsl = Convert.ToBoolean(repository.GetConfigValue("Email_EnableSsl"));
			SenderEmailAddress = repository.GetConfigValue("Email_SenderEmailAddress");
			SenderPassword = repository.GetConfigValue("Email_SenderPassword");
		}

		public void SendEmail(string name, string phoneNumber, string email, string text)
		{
			using (var client = new SmtpClient(Host, Port))
			{
				client.EnableSsl = EnableSsl;
				client.Credentials = new NetworkCredential(SenderEmailAddress, SenderPassword);
				MailMessage message = CreateMessage(name, phoneNumber, email, text);
				client.Send(message);
			}
		}

		private MailMessage CreateMessage(string name, string phoneNumber, string email, string text)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress(SenderEmailAddress),
				Subject = "Обратная связь с сайта omenergo.by",
				Body = $"Имя: {name}" + Environment.NewLine
					 + $"Телефон: {phoneNumber}" + Environment.NewLine
					 + $"Email: {email}" + Environment.NewLine 
					 + $"Сообщение: {text}"
			};
			mailMessage.To.Add(SenderEmailAddress);
			return mailMessage;
		}
	}
}
