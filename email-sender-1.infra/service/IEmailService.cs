using System.Threading.Tasks;


namespace email_sender_1.infra.service
{
  public interface IEmailService
  {
    Task<SendGrid.Response> Send(string from, string to, string subject, string body);
  }
}
