using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Shared;

public class SystemStatus
{
    [Key]
    public int id { get; set; }

    [Required]
    public bool Status { get; set; }
}