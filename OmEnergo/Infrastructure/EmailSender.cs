using Microsoft.Extensions.Localization;
using OmEnergo.Resources;
using System.Net;
using System.Net.Mail;

namespace OmEnergo.Infrastructure
{
	public class EmailSender
	{
		private readonly IStringLocalizer localizer;

		private string host;
		private int port;
		private bool enableSsl;
		private string senderEmailAddress;
		private string senderPassword;

		public EmailSender(IStringLocalizer localizer)
		{
			this.localizer = localizer;
		}

		public void InitializeConfiguration(string host, int port, bool enableSsl,
			string senderEmailAddress, string senderPassword)
		{
			this.host = host;
			this.port = port;
			this.enableSsl = enableSsl;
			this.senderEmailAddress = senderEmailAddress;
			this.senderPassword = senderPassword;
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
				Subject = localizer[nameof(SharedResource.FeedbackFromTheOmenergoBySite)],
				Body = localizer[nameof(SharedResource.FeedbackEmailBody)].Value
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
