using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using email_sender_1.AttributeCustom;
using email_sender_1.infra.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelCore=email_sender_1.core.Model;
using Service = email_sender_1.infra.service;

namespace email_sender_1.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EmailController : ControllerBase
  {
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailService _emailService;

    public EmailController(ILogger<EmailController> logger, Service.IEmailService emailService)
    {
      _logger = logger;
      _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Get()
    {
      return Ok("hello"); 
    }

    [ServiceFilter(typeof(ApiKeyAttribute))]
    [HttpPost]
    [Consumes("application/json")]
    public IActionResult Send([FromBody] ModelCore.EmailInfo emailInfo)
    {

      _logger.LogInformation("emailInfo: {@EmailInfo}", emailInfo);
      _emailService.Send(emailInfo.From, emailInfo.To, emailInfo.Subject, emailInfo.Body);

      return Ok(new { status = "Sent" });
    }
  }
}
