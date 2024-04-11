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
public class VillaAPIController : ControllerBase
{
    #region DI

    protected APIResponse _response;
    private readonly IVillaRepo _dbVilla;
    private readonly IMapper _mapper;

    public VillaAPIController(IVillaRepo dbVilla, IMapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
        this._response = new();
    }

    #endregion

    #region GET ALL

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAll();
            _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
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

    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(200, Type =typeof(VillaDTO))]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.Get(u => u.Id == id);
            if (villa is null)
            {
                return NotFound();
            }

            _response.Result = _mapper.Map<VillaDTO>(villa);
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
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
    {
        if (await _dbVilla.Get(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already Exists!");
            return BadRequest(ModelState);
        }

        if (createDTO is null)
        {
            return BadRequest(createDTO);
        }

        Villa villa = _mapper.Map<Villa>(createDTO);

        await _dbVilla.Create(villa);
        _response.Result = _mapper.Map<VillaDTO>(villa);
        _response.StatusCode = HttpStatusCode.Created;
        return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
    }

    #endregion

    #region DELETE

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.Get(u => u.Id == id);
            if (villa is null)
            {
                return NotFound();
            }

            await _dbVilla.Remove(villa);
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

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }

            Villa model = _mapper.Map<Villa>(updateDTO);


            await _dbVilla.Update(model);
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

    #region UPDATE-PATCH

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await _dbVilla.Get(u => u.Id == id, tracked: false);

        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

        if (villa == null)
        {
            return BadRequest();
        }

        patchDTO.ApplyTo(villaDTO, ModelState);
        Villa model = _mapper.Map<Villa>(villaDTO);

        await _dbVilla.Update(model);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }

    #endregion
}