using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
namespace Library.Web.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration configuration;

		public EmailSender(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				Credentials = new NetworkCredential(configuration["EmailSetting:Email"], configuration["EmailSetting:Password"]),
				EnableSsl = true
			};

			var message = new MailMessage()
			{
				From = new MailAddress(configuration["EmailSetting:Email"]),
				Subject = subject,
				Body = htmlMessage,
				IsBodyHtml =true
			};

			message.To.Add(email);

			await smtp.SendMailAsync(message);
		}
	}
}
