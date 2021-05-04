using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using email_sender_1.Utility;
using ModelCore=email_sender_1.core.Model;
using Microsoft.Extensions.Options;
using email_sender_1.core.Model;
using Microsoft.Extensions.Logging;

namespace email_sender_1.AttributeCustom
{
  [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
  public class ApiKeyAttribute : Attribute, IAsyncActionFilter
  {
    readonly ILogger<ApiKeyAttribute> _logger;

    public ApiKeyAttribute(ILogger<ApiKeyAttribute> logger)
    {
      _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      string headerKey = "x-api-key";

      var sendgridOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<EmailSenderAPIOptions>>();
      string actualApiKey = sendgridOptions.Value.ApiKey;

      if (!context.HttpContext.Request.Headers.TryGetValue(headerKey, out var apiKeyFromRequest))
      {
        context.Result = ContentResultCustom.ApiKeyNotProvided;
        return;
      }

      if (!actualApiKey.Equals(apiKeyFromRequest))
      {
        //_logger.LogInformation($"apiKeyFromRequest: {apiKeyFromRequest}");
        context.Result = ContentResultCustom.ApiKeyNotValid;
        return;
      }

      await next();
    }
  }
}
