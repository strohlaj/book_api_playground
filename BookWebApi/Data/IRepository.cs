using BookWebApi.Models;

namespace BookWebApi.Data;

public interface IRepository<T> where T: IBaseModel
{
    /// <summary>
    /// Retrieves from the repository an entity whose entity id matches `id`
    /// </summary>
    /// <param name="id"></param>
    /// <returns>T entity whose entity id matches `id`</returns>
    public T Get(Guid id);


    /// <summary>
    /// Retrieves all Entites from the repository
    /// </summary>
    /// <returns>a list of all entites in the repository of type T</returns>
    public IEnumerable<T> GetAll();

    /// <summary>
    /// Adds a new item into the repository or updates an item in the repository
    /// whose `item` id matches an Id of an entity in the repository.
    /// </summary>
    /// <param name="item"></param>
    public void Upsert(T item);

    /// <summary>
    /// Removes from the repository the entity whose Id matches `item`s Id
    /// </summary>
    /// <param name="item"></param>
    public void Delete(T item);
}