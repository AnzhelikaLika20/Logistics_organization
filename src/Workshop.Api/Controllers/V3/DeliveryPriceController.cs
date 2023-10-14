using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Workshop.Api.ActionFilters;
using Workshop.Api.Bll.Models;
using Workshop.Api.Requests.V3;
using Workshop.Api.Responses.V3;
using Workshop.Api.Validators;
using Workshop.Domain.Services.Interfaces;

namespace Workshop.Api.Controllers.V3;

[ApiController]
[Route("v3/[controller]")]
public class DeliveryPriceController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;
    private readonly IAnalyseDataService _analyseDataService;

    public DeliveryPriceController(IPriceCalculatorService priceCalculatorService,
        IAnalyseDataService analyseDataService)
    {
        _priceCalculatorService = priceCalculatorService;
        _analyseDataService = analyseDataService;
    }

    [ExceptionFilter]
    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(CalculateRequest request)
    {
        var validator = new CalculatorRequestValidator();
        await validator.ValidateAndThrowAsync(request);
        var price =
            _priceCalculatorService.CalculatePrice(
                request.Goods
                    .Select(x => new GoodModel(
                        x.Height,
                        x.Length,
                        x.Width,
                        x.Weight))
                    .ToArray(), request.distance);
        return new CalculateResponse(price);
    }

    [HttpPost("get-history")]
    public GetHistoryResponse[] GetHistory(GetHistoryRequest request)
    {
        var log = _priceCalculatorService.QueryLog(request.Take);
        return log.Select(x => new GetHistoryResponse(
                new CargoResponse(
                    x.Volume,
                    x.Weight),
                x.Price))
            .ToArray();
    }

    [HttpDelete("delete-history")]
    public DeleteHistoryResponse DeleteHistory()
    {
        _priceCalculatorService.DeleteHistory();
        return new DeleteHistoryResponse();
    }

    [HttpGet("GetStatistics")]
    public AnaliticsResponse GetStatistics()
    {
        var maxVolume = _analyseDataService.MaxVolume();
        var maxWeight = _analyseDataService.MaxWeight();
        var distanceOfMaxBiggestGood = _analyseDataService.DistanceOfBiggestGood();
        var distanceOfMaxHeaviestGood = _analyseDataService.DistanceOfHeaviestGood();

        return new AnaliticsResponse(maxVolume, maxWeight, distanceOfMaxBiggestGood, distanceOfMaxHeaviestGood);
    }
}