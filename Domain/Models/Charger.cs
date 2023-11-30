using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class Charger
{
    [Key]
    public int ChargerId { get; set; }
    
    [Required]
    public int ChargerAmpere { get; set; }
}