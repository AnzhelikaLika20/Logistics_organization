namespace Workshop.Domain.Services.Interfaces;

public interface IAnalyseDataService
{
    double MaxVolume();
    double MaxWeight();
    double DistanceOfHeaviestGood();
    double DistanceOfBiggestGood();
    decimal WeightedAverageCost();
}