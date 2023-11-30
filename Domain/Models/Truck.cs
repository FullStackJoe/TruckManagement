using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class Truck
{
    [Key]
    public int TruckId { get; set; }
    
    [Required]
    public string TruckType { get; set; }
    
    [Required]
    public int BatterySizeAh { get; set; }
}