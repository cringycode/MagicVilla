using MagicVilla_Utility_Fixed;

namespace MagicVilla_Web_Fixed.Models;

public class APIRequest
{
    public SD.ApiType ApiType { get; set; } = SD.ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
}