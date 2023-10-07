using Workshop.Api.Bll.Services;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.HostedServices;

public class GoodSyncHostedService: BackgroundService
{
    private readonly IGoodRepository _repository;
    private readonly IGoodsService _service;

    public GoodSyncHostedService(IGoodRepository repository, IGoodsService service)
    {
        _repository = repository;
        _service = service;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var goods = _service.GetGoods().ToList();
            foreach (var good in goods)     
            {
                _repository.AddOrUpdate(good);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }

}