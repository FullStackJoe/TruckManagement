using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class Measurement
{
    [Key]
    public int MeasurementId { get; set; }

    public int ChargerId { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public int MeasurementKWH { get; set; }
}