using Workshop.Api.Dal.Entities;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.Dal.Repositories;

public sealed class GoodRepository : IGoodRepository
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
}