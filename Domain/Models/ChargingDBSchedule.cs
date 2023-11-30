using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class ChargingDBSchedule
{
    [Key] public int ChargingDBScheduleId { get; set; }

    [Required] public int ChargerId { get; set; }

    public DateTime startHour { get; set; }
}