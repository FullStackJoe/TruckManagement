using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class WallCharger
{
    [Key]
    public int ChargerId { get; set; }
    
    [Required]
    public int ChargerAmpere { get; set; }
}