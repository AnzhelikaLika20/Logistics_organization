using Workshop.Domain.Entities;
using Workshop.Domain.Separated;

namespace Workshop.Infrastructure.Dal.Repositories;

internal sealed class GoodRepository : IGoodRepository
{
    private readonly Dictionary<int, GoodEntity> _store = new();

    public void AddOrUpdate(GoodEntity entity)
    {
        if (_store.ContainsKey(entity.Id))
            _store.Remove(entity.Id);
        
        _store.Add(entity.Id, entity);
    }

    public ICollection<GoodEntity> GetAll()
    {
        return _store.Select(x => x.Value).ToArray();
    }

    public GoodEntity Get(int id)
    {
        return _store[id];
    }
}