using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using email_sender_1.core.Model;
using Service = email_sender_1.infra.service;
using ModelCore = email_sender_1.core.Model;



namespace email_sender_1.test
{
  public class CommonFixture : IDisposable
  {
    public CommonFixture()
    {
      var serviceCollection = new ServiceCollection();
      var configRoot = GetConfigurationRoot();

      var sendgridOptions = new ModelCore.SendgridOptions();      
      configRoot.GetSection(ModelCore.SendgridOptions.Position).Bind(sendgridOptions);

      serviceCollection.AddScoped<Service.IEmailService>(
          p => {

            return new Service.EmailService(sendgridOptions.ApiKey);
          });

      ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    IConfiguration GetConfigurationRoot()
    {
      return new ConfigurationBuilder()
       .AddJsonFile("appsettings.json.test", optional: true)
       .Build();
    }

    public ServiceProvider ServiceProvider { get; private set; }

    public void Dispose()
    {

    }


  }
}
