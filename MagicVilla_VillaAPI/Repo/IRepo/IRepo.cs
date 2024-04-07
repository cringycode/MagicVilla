using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repo.IRepo;

public interface IRepo<T> where T : class
{
    Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
    Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true);
    Task Create(T entity);
    Task Remove(T entity);
    Task Save();
}