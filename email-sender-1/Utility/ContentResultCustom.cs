using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace email_sender_1.Utility
{
  public class ContentResultCustom
  {
    public static ContentResult ApiKeyNotProvided => new ContentResult()
    {
      StatusCode = 401,
      Content = "Api Key was not provided."
    };

    public static ContentResult ApiKeyNotValid => new ContentResult()
    {
      StatusCode = 401,
      Content = "Not a valid Api Key."
    };
  }
}
