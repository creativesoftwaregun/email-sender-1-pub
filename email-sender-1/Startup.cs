using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

using ModelCore =email_sender_1.core.Model;
using Service= email_sender_1.infra.service;
using email_sender_1.AttributeCustom;

namespace email_sender_1
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<ModelCore.SendgridOptions>
        (Configuration.GetSection(ModelCore.SendgridOptions.Position));
      services.Configure<ModelCore.EmailSenderAPIOptions>
        (Configuration.GetSection(ModelCore.EmailSenderAPIOptions.Position));
      services.Configure<ModelCore.AzureOptions>
        (Configuration.GetSection(ModelCore.AzureOptions.Position));
      services.AddScoped<ApiKeyAttribute>();

      services.AddControllers();

      services.AddScoped<Service.IEmailService>(
        p=> {
          var options = p.GetRequiredService<IOptions<ModelCore.SendgridOptions>>();
          return new Service.EmailService(options.Value.ApiKey);
        });

      services.AddSwaggerGen();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
      }

      app.UseHttpsRedirection();
      app.UseSerilogRequestLogging();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
