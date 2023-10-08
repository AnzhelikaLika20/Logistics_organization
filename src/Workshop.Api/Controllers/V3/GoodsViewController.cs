using Microsoft.AspNetCore.Mvc;
using Workshop.Api.Dal.Repositories.Interfaces;
using Workshop.Api.ViewModels;

namespace Workshop.Api.Controllers.V3;

public class GoodsViewController : Controller
{
    private readonly IGoodRepository _repository;

    public GoodsViewController(IGoodRepository repository)
    {
        _repository = repository;
    }
    public IActionResult Index()
    {
        var entities = _repository.GetAll();
        var viewModel = new GoodsPageViewModel();
        viewModel.Goods = entities.Select(x => new GoodViewModel(
            x.Name,
            x.Id,
            x.Length,
            x.Width,
            x.Height,
            x.Width,
            x.Count,
            x.Price)).ToArray();
        return View("/Views/GoodsPageView.cshtml", viewModel);
    }
}