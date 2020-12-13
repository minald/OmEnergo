using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure.Database;
using System;
using System.Net;
using System.Net.Mail;

namespace OmEnergo.Infrastructure
{
	public class EmailSender
	{
		private readonly IStringLocalizer localizer;

		private readonly string host;
		private readonly int port;
		private readonly bool enableSsl;
		private readonly string senderEmailAddress;
		private readonly string senderPassword;

		public EmailSender(IStringLocalizer localizer, ConfigKeyRepository configKeyRepository)
		{
			this.localizer = localizer;

			host = configKeyRepository.GetConfigValue("Email_Host");
			port = Convert.ToInt32(configKeyRepository.GetConfigValue("Email_Port"));
			enableSsl = Convert.ToBoolean(configKeyRepository.GetConfigValue("Email_EnableSsl"));
			senderEmailAddress = configKeyRepository.GetConfigValue("Email_SenderEmailAddress");
			senderPassword = configKeyRepository.GetConfigValue("Email_SenderPassword");
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
				Subject = localizer["FeedbackFromTheOmenergoBySite"],
				Body = localizer["FeedbackEmailBody"].Value
					.Replace("{{name}}", name)
					.Replace("{{phoneNumber}}", phoneNumber)
					.Replace("{{email}}", email)
					.Replace("{{text}}", text)
			};

			mailMessage.To.Add(senderEmailAddress);
			return mailMessage;
		}
	}
}
