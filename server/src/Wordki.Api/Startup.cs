﻿using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Wordki.Infrastructure.Framework.ExceptionMiddleware;
using Wordki.Core;
using Wordki.Commands;
using Wordki.Queries;
using Wordki.Infrastructure;
using Microsoft.Extensions.Hosting;
using Wordki.Infrastructure.Framework.HandleTimeMiddleware;
using Wordki.Infrastructure.Services;
using MediatR.Extensions.Autofac.DependencyInjection;
using Wordki.Api.Repositories.EntityFrameworkRepositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Wordki
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .OptionConfig(Configuration)
                .JwtConfig(Configuration)
                .CorsConfig()
                .LoggingConfig(Configuration)
                .ServicesConfig()
                .AddDbContext<WordkiDbContext>()
                .AddMvc(o => { 
                    o.EnableEndpointRouting = false;
                    o.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<CommandModule>();
            builder.RegisterModule<QueryModule>();
            builder.AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("AllowAll");
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<HandleTimeMiddleware>();
            app.UseAuthentication();
            app.UseMvc();

            //if (Configuration.GetValue<bool>("General:Mocks"))
            //{
            //    var initializer = app.ApplicationServices.GetService<IDataInitializer>();
            //    initializer.Initialize().Wait();
            //}

            if (Configuration.GetValue<bool>("General:Mocks"))
            {
                var initializer = app.ApplicationServices.GetService<IDatabaseInitializer>();
                initializer.Init().Wait();
            }

            var appLifeTime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            appLifeTime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }

    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
