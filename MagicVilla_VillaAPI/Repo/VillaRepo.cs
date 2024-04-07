using System.Linq.Expressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repo.IRepo;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repo;

public class VillaRepo : Repo<Villa>, IVillaRepo
{
    #region DI

    private readonly AppDbContext _db;

    public VillaRepo(AppDbContext db) : base(db)
    {
        _db = db;
    }

    #endregion

    public async Task<Villa> Update(Villa entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _db.Villas.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}