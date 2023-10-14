using Workshop.Domain.Entities;
using Workshop.Domain.Services.Interfaces;

namespace Workshop.Infrastructure
{

    public class GoodsService : IGoodsService
    {

        private readonly List<ItemModel> _goods = new()
        {
            new("помада", 1, 1000, 2000, 3000, 4000, 100),
            new("телевизор", 2, 6, 34, 9, 65, 17),
            new("Зеркало", 4, 873, 87, 452, 23, 29),
            new("Шкаф", 5, 356, 123, 98, 133, 120),
            new("куртка", 7, 34566, 455, 87, 65, 43)
        };

        public IEnumerable<GoodEntity> GetGoods()
        {
            var rnd = new Random();
            foreach (var model in _goods)
            {
                var count = rnd.Next(0, 10);
                yield return new GoodEntity(
                    model.Name,
                    model.Id,
                    model.Length,
                    model.Width,
                    model.Height,
                    model.Weight,
                    count,
                    model.Price);
            }
        }
    }

    public sealed record ItemModel(
        string Name,
        int Id,
        int Length,
        int Width,
        int Height,
        double Weight,
        decimal Price);
}