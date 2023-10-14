using Workshop.Domain.Entities;

namespace Workshop.Domain.Separated;

public interface IGoodRepository
{
    public void AddOrUpdate(GoodEntity entity);
    public ICollection<GoodEntity> GetAll();
    public GoodEntity Get(int id);
}