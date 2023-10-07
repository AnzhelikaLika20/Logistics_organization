using Workshop.Api.Bll;
using Workshop.Api.Bll.Services;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Dal.Repositories;
using Workshop.Api.Dal.Repositories.Interfaces;

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
        services.Configure<PriceCalculatorOptions>(_configuration.GetSection("PriceCalculatorOptions"));
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        //Avoiding name conflicts in swagger 
        services.AddSwaggerGen(a => { a.CustomSchemaIds(x => x.FullName); });
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddScoped<IAnalyseDataService, AnalyseDataService>();
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

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}