using AutoMapper;
using MagicVilla_Web_Fixed.Models;
using MagicVilla_Web_Fixed.Models.DTO;
using MagicVilla_Web_Fixed.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web_Fixed.Controllers;

public class VillaController : Controller
{
    #region DI

    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }

    #endregion

    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDTO> list = new();

        var response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }
}