using Workshop.Domain.Entities;
using Workshop.Domain.Separated;

namespace Workshop.Infrastructure.Dal.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly List<StorageEntity> _storage = new ();
    
    public void Save(StorageEntity entity)
    {
        _storage.Add(entity);
    }

    public StorageEntity[] Query()
    {
        return _storage.ToArray();
    }

    public void DeleteHistory()
    {
        _storage.Clear();
    }
}