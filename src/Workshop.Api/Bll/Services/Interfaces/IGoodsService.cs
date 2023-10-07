using Workshop.Api.Dal.Entities;

namespace Workshop.Api.Bll.Services;

public interface IGoodsService
{
    IEnumerable<GoodEntity> GetGoods();
}