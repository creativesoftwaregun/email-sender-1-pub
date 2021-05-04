using System;
using System.Threading.Tasks;
using email_sender_1.core.Model;
using Model = email_sender_1.core.Model;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace email_sender_1.infra.service
{

  public class EmailService : IEmailService
  {
    private readonly string _apiKey;

    public EmailService(string apiKey)
    {
      _apiKey = apiKey;
    }

    public async Task<SendGrid.Response> Send(string from, string to, string subject, string body)
    {
      var client = new SendGridClient(_apiKey);
      var fromEmail = new EmailAddress(from, from);
      var toEmail = new EmailAddress(to, to);
      var msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, body, body);
      return await client.SendEmailAsync(msg);
    }
  }
}
