using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO;

public class VillaDTO
{
    public int Id { get; set; }
    [MaxLength(30)] public required string Name { get; set; }
    public int Occupancy { get; set; }
    public int Sqm { get; set; }
}