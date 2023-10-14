using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Workshop.Domain.Separated;
using Workshop.Domain.Services.Interfaces;
using Workshop.Domain.Services;

namespace Workshop.Domain.DependencyInjection.Extensions;

public static class DomainServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PriceCalculatorOptions>(configuration.GetSection("PriceCalculatorOptions"));
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>(x =>
        {
            var options = x.GetRequiredService<IOptionsSnapshot<PriceCalculatorOptions>>().Value;
            return new PriceCalculatorService(options, x.GetRequiredService<IStorageRepository>());
        });
        return services;
    }
}