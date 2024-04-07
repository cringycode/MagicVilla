using System.Linq.Expressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repo.IRepo;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repo;

public class Repo<T> : IRepo<T> where T : class
{
    #region DI

    private readonly AppDbContext _db;
    internal DbSet<T> _dbSet;

    public Repo(AppDbContext db)
    {
        _db = db;
        this._dbSet = _db.Set<T>();
    }

    #endregion

    public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        await Save();
    }
    
    public async Task Remove(T entity)
    {
        _dbSet.Remove(entity);
        await Save();
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}