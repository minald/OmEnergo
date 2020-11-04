using OmEnergo.Infrastructure.Database;
using System;
using System.Net;
using System.Net.Mail;

namespace OmEnergo.Infrastructure
{
	public class EmailSender
	{
		private readonly string host;
		private readonly int port;
		private readonly bool enableSsl;
		private readonly string senderEmailAddress;
		private readonly string senderPassword;

		public EmailSender(Repository repository)
		{
			host = repository.GetConfigValue("Email_Host");
			port = Convert.ToInt32(repository.GetConfigValue("Email_Port"));
			enableSsl = Convert.ToBoolean(repository.GetConfigValue("Email_EnableSsl"));
			senderEmailAddress = repository.GetConfigValue("Email_SenderEmailAddress");
			senderPassword = repository.GetConfigValue("Email_SenderPassword");
		}

		public void SendEmail(string name, string phoneNumber, string email, string text)
		{
			using var client = new SmtpClient(host, port)
			{
				EnableSsl = enableSsl,
				Credentials = new NetworkCredential(senderEmailAddress, senderPassword)
			};
			var message = CreateMessage(name, phoneNumber, email, text);
			client.Send(message);
		}

		private MailMessage CreateMessage(string name, string phoneNumber, string email, string text)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress(senderEmailAddress),
				Subject = "Обратная связь с сайта omenergo.by",
				Body = $"Имя: {name}" + Environment.NewLine
					 + $"Телефон: {phoneNumber}" + Environment.NewLine
					 + $"Email: {email}" + Environment.NewLine 
					 + $"Сообщение: {text}"
			};
			mailMessage.To.Add(senderEmailAddress);
			return mailMessage;
		}
	}
}
