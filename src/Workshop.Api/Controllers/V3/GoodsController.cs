using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Workshop.Api.Bll.Models;
using Workshop.Api.Bll.Services;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Dal.Entities;
using Workshop.Api.Dal.Repositories.Interfaces;
using Workshop.Api.Responses.V3;

namespace Workshop.Api.Controllers.V3;

[ApiController]
[Route("goods")]
public class GoodsController : Controller
{
    private readonly IGoodRepository _repository;
    private readonly ILogger<GoodsController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoodsController(IGoodRepository repository,
        ILogger<GoodsController> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public ICollection<GoodEntity> GetAll()
    {
        return _repository.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<CalculateResponse> Calculate(
        [FromServices] IPriceCalculatorService
            priceCalculatorService,
        int id)
    {
        
        //если удалить параметры метода, и сериализовать запрос ниже, тк Body читается только один раз(те во время запроса)
        /*
        _httpContextAccessor.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        var sr = new StreamReader(_httpContextAccessor.HttpContext.Request.Body);
        var bodyString = await sr.ReadToEndAsync();
        _logger.LogInformation(bodyString);
        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var request = JsonSerializer.Deserialize<CalculateResponse>(bodyString, options);*/
        
        var good = _repository.Get(id);
        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight);
        var price =
            priceCalculatorService.CalculatePrice(new[] { model });
        return new CalculateResponse(price);
    }
}