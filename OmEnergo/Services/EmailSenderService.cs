using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using System;

namespace OmEnergo.Services
{
	public class EmailSenderService
	{
		private readonly EmailSender emailSender;

		public EmailSenderService(ConfigKeyRepository configKeyRepository, EmailSender emailSender)
		{
			string host = configKeyRepository.GetConfigValue("Email_Host");
			int port = Convert.ToInt32(configKeyRepository.GetConfigValue("Email_Port"));
			bool enableSsl = Convert.ToBoolean(configKeyRepository.GetConfigValue("Email_EnableSsl"));
			string senderEmailAddress = configKeyRepository.GetConfigValue("Email_SenderEmailAddress");
			string senderPassword = configKeyRepository.GetConfigValue("Email_SenderPassword");

			this.emailSender = emailSender;
			this.emailSender.InitializeConfiguration(host, port, enableSsl, senderEmailAddress, senderPassword);
		}

		public void SendEmail(string name, string phoneNumber, string email, string text)
		{
			emailSender.SendEmail(name, phoneNumber, email, text);
		}
	}
}
