using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repo.IRepo;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/VillaAPI")]
[ApiController]
public class VillaNumberAPIController : ControllerBase
{
    #region DI

    protected ApiResponse _response;
    private readonly IVillaNumberRepo _dbVillaNumber;
    private readonly IVillaRepo _dbVilla;
    private readonly IMapper _mapper;

    public VillaNumberAPIController(IVillaNumberRepo dbVillaNumber, IMapper mapper, IVillaRepo dbVilla)
    {
        _dbVillaNumber = dbVillaNumber;
        _mapper = mapper;
        _dbVilla = dbVilla;
        this._response = new();
    }

    #endregion

    #region GET ALL

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> GetVillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAll();
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    #endregion

    #region GET

    [HttpGet("{id:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villaNumber = await _dbVillaNumber.Get(u => u.VillaNo == id);
            if (villaNumber is null)
            {
                return NotFound();
            }

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    #endregion

    #region CREATE

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
    {
        if (await _dbVillaNumber.Get(u => u.VillaNo == createDTO.VillaNo) != null)
        {
            ModelState.AddModelError("CustomError", "Villa Number already Exists!");
            return BadRequest(ModelState);
        }

        if (await _dbVilla.Get(u=>u.Id == createDTO.VillaID) == null)
        {
            ModelState.AddModelError("CustomError", "Villa ID already Exists!");
            return BadRequest(ModelState);
        }

        if (createDTO is null)
        {
            return BadRequest(createDTO);
        }

        VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

        await _dbVillaNumber.Create(villaNumber);
        _response.Result = _mapper.Map<VillaDTO>(villaNumber);
        _response.StatusCode = HttpStatusCode.Created;
        return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
    }

    #endregion

    #region DELETE

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
    public async Task<ActionResult<ApiResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villaNumber = await _dbVillaNumber.Get(u => u.VillaNo == id);
            if (villaNumber is null)
            {
                return NotFound();
            }

            await _dbVillaNumber.Remove(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    #endregion

    #region UPDATE-PUT

    [HttpPut("{id:int}", Name = "UpdateVillavillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> UpdateVillavillaNumber(int id,
        [FromBody] VillaNumberUpdateDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || id != updateDTO.VillaNo)
            {
                return BadRequest();
            }
            if (await _dbVilla.Get(u=>u.Id == updateDTO.VillaID) == null)
            {
                ModelState.AddModelError("CustomError", "Villa ID already Exists!");
                return BadRequest(ModelState);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);


            await _dbVillaNumber.Update(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    #endregion
}