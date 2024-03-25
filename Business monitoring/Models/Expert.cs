using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class Expert : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Salt { get; set; }
    [Required] public int Level { get; set; }
    [Required] public bool IsBlocked { get; set; }
    [Required] public bool IsDeleted { get; set; }
}