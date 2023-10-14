using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Workshop.Api.ActionFilters;
using Workshop.Api.HostedServices;
using Workshop.Api.Middlewares;
using Workshop.Domain;
using Workshop.Domain.DependencyInjection.Extensions;
using Workshop.Domain.Separated;
using Workshop.Domain.Services;
using Workshop.Domain.Services.Interfaces;
using Workshop.Infrastructure;
using Workshop.Infrastructure.Dal.Repositories;

namespace Workshop.Api;

public sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Startup(IConfiguration configuration,
        IHostEnvironment hostEnvironment,
        IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
        _webHostEnvironment = webHostEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //для решения проблемы с неймингом во время деcериализиции
        //services.AddMcv().AddJsonOptions(x => x.JsonSerializerOptions...
        services
            .AddDomain(_configuration)
            .AddInfrastructure()
            .AddControllers()
            .AddMvcOptions(ConfigureMvc)
            .Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(a => { a.CustomSchemaIds(x => x.FullName); })
            .AddScoped<IAnalyseDataService, AnalyseDataService>()
            .AddHostedService<GoodSyncHostedService>().AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    private static void ConfigureMvc(MvcOptions x)
    {
        x.Filters.Add(new ExceptionFilterAttribute());
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.OK));
        //навесили фильтры на все приложение
    }

    public void Configure(IHostEnvironment environment, IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseMiddleware<ErrorMiddleware>();
        //для десериализации
        //позволяет читать ответ несколько раз
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next.Invoke();
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute(); //для подключения view
            endpoints.MapControllerRoute("goods-page", "goods-page", new
            {
                Controller = "GoodsView",
                Action = "Index"
            });
        });
    }
}