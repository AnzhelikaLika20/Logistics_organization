using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Workshop.Api.ActionFilters;
using Workshop.Api.HostedServices;
using Workshop.Api.Middlewares;
using Workshop.Domain;
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
        services.AddMvc()
            .AddMvcOptions(x =>
            {
                x.Filters.Add(new ExceptionFilterAttribute());
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.OK));

            });//навесили фильтры на все приложение
        services.Configure<PriceCalculatorOptions>(_configuration.GetSection("PriceCalculatorOptions"));
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        //Avoiding name conflicts in swagger 
        services.AddSwaggerGen(a => { a.CustomSchemaIds(x => x.FullName); });
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>(x =>
        {
            var options = x.GetRequiredService<IOptionsSnapshot<PriceCalculatorOptions>>().Value;
            return new PriceCalculatorService(options, x.GetRequiredService<IStorageRepository>());
        });
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddScoped<IAnalyseDataService, AnalyseDataService>();
        services.AddSingleton<IGoodRepository, GoodRepository>();
        services.AddHostedService<GoodSyncHostedService>();
        services.AddSingleton<IGoodsService, GoodsService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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