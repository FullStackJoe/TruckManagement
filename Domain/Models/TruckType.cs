using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class TruckType
{
    [Key]
    public int TruckTypeId { get; set; }
    
    [Required]
    public string Model { get; set; }
    
    [Required]
    public int BatterySizeAh { get; set; }
}