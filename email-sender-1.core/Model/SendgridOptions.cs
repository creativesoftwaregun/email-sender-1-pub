using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace email_sender_1.core.Model
{
  public class SendgridOptions 
  {
    public static string Position = "Sendgrid";
    public string ApiKey { get; set; }
  }
}
