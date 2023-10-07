using Microsoft.AspNetCore.Mvc;
using Workshop.Api.Dal.Entities;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.Controllers.V3;

[ApiController]
[Route("goods")]
public class GoodsController : Controller
{

    private readonly IGoodRepository _repository;
    public GoodsController(IGoodRepository repository)
    {
        _repository = repository;
    }
    [HttpGet]
    public ICollection<GoodEntity> GetAll()
    {
        return _repository.GetAll();
    }
}