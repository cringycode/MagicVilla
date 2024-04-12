using MagicVilla_Web_Fixed.Models;

namespace MagicVilla_Web_Fixed.Services.IServices;

public interface IBaseService
{
    APIResponse responseModel { get; set; }
    Task<T> SendAsync<T>(APIRequest apiRequest);
}