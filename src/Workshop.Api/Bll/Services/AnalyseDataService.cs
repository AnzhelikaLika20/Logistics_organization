using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.Bll.Services;

public class AnalyseDataService : IAnalyseDataService
{
    private readonly IStorageRepository _storageRepository;

    public AnalyseDataService(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }
    
    public double MaxVolume()
    {
        var query = _storageRepository.Query();
        var result = query.Max(x => x.MaxVolume);
        return result;
    }

    public double MaxWeight()
    {
        var query = _storageRepository.Query();
        var result = query.Max(x => x.MaxWeight);
        return result;
    }

    public double DistanceOfHeaviestGood()
    {
        var query = _storageRepository.Query();
        var result = query.FirstOrDefault(x => x.MaxWeight == MaxWeight())?.Distance ?? 0;
        return result;
    }

    public double DistanceOfBiggestGood()
    {
        var query = _storageRepository.Query();
        var result = query.FirstOrDefault(x => x.MaxVolume == MaxVolume())?.Distance ?? 0;
        return result;
    }

    public decimal WeightedAverageCost()
    {
        var query = _storageRepository.Query();
        var sum = query.Sum(x => x.Price);
        var count = query.Sum(x => x.Count);
        return (decimal)sum / count;
    }
}