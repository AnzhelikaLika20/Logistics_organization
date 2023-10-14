using Microsoft.Extensions.DependencyInjection;
using Workshop.Domain.Separated;
using Workshop.Domain.Services.Interfaces;
using Workshop.Infrastructure.Dal.Repositories;

namespace Workshop.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IGoodRepository, GoodRepository>();
        services.AddSingleton<IGoodsService, GoodsService>();
        return services;
    }
}