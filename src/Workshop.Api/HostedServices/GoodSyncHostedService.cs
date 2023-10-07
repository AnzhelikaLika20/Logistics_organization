using Workshop.Api.Bll.Services;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.HostedServices;

public class GoodSyncHostedService: IHostedService
{
    private readonly IGoodRepository _repository;
    private readonly IGoodsService _service;

    public GoodSyncHostedService(IGoodRepository repository, IGoodsService service)
    {
        _repository = repository;
        _service = service;
    }
    
    private async Task ExecuteAsync()
    {
        while (true)
        {
            var goods = _service.GetGoods().ToList();
            foreach (var good in goods)     
            {
                _repository.AddOrUpdate(good);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        ExecuteAsync();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}