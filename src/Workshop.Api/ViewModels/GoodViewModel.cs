namespace Workshop.Api.ViewModels;


public class GoodsPageViewModel
{
    public ICollection<GoodViewModel> Goods { get; set; }
}
public record GoodViewModel(
    string Name,
    int Id,
    int Length,
    int Width, 
    int Height, 
    double Weight,
    int Count,
    decimal Price);