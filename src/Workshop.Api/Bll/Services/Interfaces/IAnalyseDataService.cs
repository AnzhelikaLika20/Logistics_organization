namespace Workshop.Api.Bll.Services.Interfaces;

public interface IAnalyseDataService
{
    double MaxVolume();
    double MaxWeight();
    double DistanceOfHeaviestGood();
    double DistanceOfBiggestGood();
    decimal WeightedAverageCost();
}