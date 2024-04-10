using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.DTO;

public class VillaNumberCreateDTO
{
    public required int VillaNo { get; set; }
    public required int VillaID { get; set; }

    public string SpecialDetails { get; set; } 
}