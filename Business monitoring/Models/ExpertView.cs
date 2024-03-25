using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class ExpertView : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string View { get; set; }
    [Required] public Business Business { get; set; }
    [Required] public Expert Expert { get; set; }
}