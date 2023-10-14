using Workshop.Domain.Entities;

namespace Workshop.Domain.Services.Interfaces;

public interface IGoodsService
{
    IEnumerable<GoodEntity> GetGoods();
}