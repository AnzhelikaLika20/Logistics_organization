using System.ComponentModel.DataAnnotations;

namespace Workshop.Api.Requests.V3;

public record GoodProperties(
    //[Range(1, int.MaxValue)]
    int Length, 
    //[Range(1, int.MaxValue)]
    int Width, 
    //[Range(1, int.MaxValue)]
    int Height, 
    //[Range(1, int.MaxValue)]
    double Weight);