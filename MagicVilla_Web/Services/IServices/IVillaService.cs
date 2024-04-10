using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaService
{
    Task<T> GetAll<T>();
    Task<T> Get<T>(int id);
    Task<T> Create<T>(VillaCreateDTO dto);
    Task<T> Update<T>(VillaUpdateDTO dto);
    Task<T> Delete<T>(int id);
}