using Workshop.Api.Bll;
using Workshop.Api.Bll.Models;
using Workshop.Domain.Entities;
using Workshop.Domain.Separated;
using Workshop.Domain.Services.Interfaces;

namespace Workshop.Domain.Services;

internal sealed class PriceCalculatorService : IPriceCalculatorService
{
    private readonly double _volumeRatio;
    private readonly double _weightRatio;

    private readonly IStorageRepository _storageRepository;

    public PriceCalculatorService(
        PriceCalculatorOptions options,
        IStorageRepository storageRepository)
    {
        _volumeRatio = options.VolumeRatio;
        _weightRatio = options.WeightRatio;
        _storageRepository = storageRepository;
    }

    public double CalculatePrice(GoodModel[] goods, double distance = 1)
    {
        if (!goods.Any())
        {
            throw new ArgumentException("Cписок товаров пустой");
        }

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);
        var resultPrice = Math.Max(volumePrice, weightPrice) * distance;
        var maxVolume = goods.Max(x => x.Height * x.Length * x.Width);
        var maxWeight = goods.Max(x => x.Weight);

        _storageRepository.Save(new StorageEntity(volume, resultPrice, DateTime.UtcNow, weight, maxVolume, maxWeight, distance, goods.Length));

        return resultPrice;
    }

    private double CalculatePriceByVolume(GoodModel[] goods, out double volume)
    {
        volume = goods.Sum(x => x.Height * x.Length * x.Width);
        var volumePrice = volume * _volumeRatio / 1000.0d;
        return volumePrice;
    }

    private double CalculatePriceByWeight(GoodModel[] goods, out double weight)
    {
        weight = goods.Sum(x => x.Weight);
        var weightPrice = weight * _weightRatio / 1000.0d;
        return weightPrice;
    }

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), take, "Take должно быть больше 0");
        }

        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();
        return log.Select(x => new CalculationLogModel(
                x.Volume,
                x.Price,
                x.Weight))
            .ToArray();
    }

    public void DeleteHistory()
    {
        _storageRepository.DeleteHistory();
    }
}