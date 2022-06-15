using BookWebApi.Models;

namespace BookWebApi.Data;

public  interface IPersistence<T> where T : IBaseModel
{
    public void Commit(IRepository<T> repo);
}