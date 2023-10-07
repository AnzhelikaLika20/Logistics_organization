namespace Workshop.Api.Dal.Entities;

public sealed record GoodEntity(
    string Name,
    int Id,
    int Length,
    int Width, 
    int Height, 
    double Weight,
    int Count,
    decimal Price);