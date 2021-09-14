using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Wordki.Infrastructure.Framework.ExceptionMiddleware;
using Wordki.Infrastructure.Framework.HandleTimeMiddleware;
using Wordki.Api.Repositories.EntityFrameworkRepositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace Wordki
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .OptionConfig(configuration)
                .JwtConfig(configuration)
                .CorsConfig()
                .LoggingConfig(configuration)
                .ServicesConfig(configuration)
                .AddDbContext<WordkiDbContext>()
                .AddMediatR(typeof(Startup).Assembly)
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddControllers();
            services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wordki", Version = "v1" });
            c.CustomSchemaIds(type => type.ToString());
        });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wordki v1"));

            app.UseCors("AllowAll");
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<HandleTimeMiddleware>();
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
