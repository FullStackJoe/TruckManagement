using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class Settings
{
    [Key]
    public int SettingsId { get; set; }

    [Required]
    public bool SmartCharging { get; set; }
    
    [Required]
    public int DailyDeadline { get; set; }
}