using Workshop.Domain.Entities;

namespace Workshop.Domain.Separated;

public interface IStorageRepository
{
    void Save(StorageEntity entity);

    StorageEntity[] Query();

    void DeleteHistory();
}