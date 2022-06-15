using BookWebApi.Models;

namespace BookWebApi.Data;

/// <summary>
/// A simple, in memory repository whose backing store items are made distinct by the BaseModel Id property.
/// A dictionary would be a valid structure to store the set and would make upsert instructions simplified.
/// however using the Guid as the key would require syncing T's Id with the keys Id and thereby would require more space.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InMemoryRepository<T> : IRepository<T> where T : IBaseModel
{
    private readonly IList<T> _source;
    private readonly IPersistence<T> _persistor;

    public InMemoryRepository(IList<T> source, IPersistence<T> persistor)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = persistor ?? throw new ArgumentNullException(nameof(persistor));
        _source = source;
        _persistor = persistor;
    }

    public T Get(Guid id)
    {
        return _source.FirstOrDefault(item => item.GetId() == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _source;
    }

    public void Upsert(T item)
    {
        // Until !! syntax comes to C# 10 manually null-check arguments.
        _ = item ?? throw new ArgumentNullException(nameof(item));
        var previousItem = _source.FirstOrDefault(t => t.GetId() == item.GetId());
        if (previousItem is not null)
        {
            _source.Remove(previousItem);
        }
        _source.Add(item);
        _persistor.Commit(this);
    }

    public void Delete(T item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        if (_source.Any(t => t.GetId() == item.GetId()))
        {
             _source.Remove(Get(item.GetId()));
             _persistor.Commit(this);
        }
    }
}