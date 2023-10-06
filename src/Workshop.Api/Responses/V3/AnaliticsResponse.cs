namespace Workshop.Api.Responses.V3;

public record AnaliticsResponse(double MaxVolume,
    double MaxWeight,
    double DistanceOfBiggestGood,
    double DistanceOfHeaviestGood);