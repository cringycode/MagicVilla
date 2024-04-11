using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repo.IRepo;

public interface IVillaNumberRepo :IRepo<VillaNumber>
{
    Task<VillaNumber> Update(VillaNumber entity);
}