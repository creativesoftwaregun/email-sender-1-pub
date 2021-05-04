using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelCore=email_sender_1.core.Model;
using Serilog;

namespace email_sender_1
{
  public class Program
  {
    public static int Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
                      .WriteTo.Console()
                      .CreateBootstrapLogger();

      Log.Information("Starting up!");

      try
      {
        CreateHostBuilder(args).Build().Run();

        Log.Information("Stopped cleanly");
        return 0;
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
        return 1;
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var config = new ConfigurationBuilder()
      .AddJsonFile($"appsettings.{environment}.json", optional: false)
      .Build();

      var azureOptions = new ModelCore.AzureOptions();
      config.GetSection("Azure").Bind(azureOptions);

      return Host.CreateDefaultBuilder(args)
           .UseSerilog((context, services, configuration) => configuration
                   .ReadFrom.Configuration(context.Configuration)
                   .ReadFrom.Services(services)
                   .Enrich.FromLogContext()
                   .WriteTo.Console()
                   .WriteTo.AzureBlobStorage(connectionString: azureOptions.EmailSenderPrimary_BlobStorageConnectionString
                        ,storageContainerName: azureOptions.ContainerName)
           )
           .ConfigureLogging(logging =>
           {
             logging.ClearProviders();
             logging.AddConsole();
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
             webBuilder.UseStartup<Startup>();
           });
    }
       
  }
}
