using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Service=email_sender_1.infra.service;
using ModelCore=email_sender_1.core.Model;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using email_sender_1.core.Model;
using Microsoft.Extensions.DependencyInjection;

namespace email_sender_1.test.integration
{
  public class EmailServiceTest : IClassFixture<CommonFixture>
  {
    private readonly ITestOutputHelper _output;
    private readonly CommonFixture _commonFixture;

    public EmailServiceTest(ITestOutputHelper output, CommonFixture commonFixture)
    {
      _output = output;
      _commonFixture = commonFixture;
    }

    [Fact]
    public async Task Send()
    {

      var emailService = _commonFixture.ServiceProvider.GetRequiredService<Service.IEmailService>();
      var from = "test@yopmail.com";
      var to= "test2@yopmail.com";
      var subject = "hello there";
      var body = "hello";
      var response = await emailService.Send(from, to, subject, body);
      _output.WriteLine(response.StatusCode.ToString());
      Assert.True(response.IsSuccessStatusCode);
      
    }
  }
}
