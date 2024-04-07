using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repo.IRepo;

public interface IVillaRepo :IRepo<Villa>
{
    Task<Villa> Update(Villa entity);
}