using System.Linq.Expressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repo.IRepo;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repo;

public class VillaNumberRepo : Repo<VillaNumber>, IVillaNumberRepo
{
    #region DI

    private readonly AppDbContext _db;

    public VillaNumberRepo(AppDbContext db) : base(db)
    {
        _db = db;
    }

    #endregion

    public async Task<VillaNumber> Update(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _db.VillaNumbers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
} 