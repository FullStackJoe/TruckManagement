using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class Measurement
{
    [Key]
    public int MeasurementId { get; set; }

    [Required]
    public int ChargerId { get; set; }
    
    [Required]
    public DateTime TimeStamp { get; set; }
    
    [Required]
    public int MeasurementKWH { get; set; }
}