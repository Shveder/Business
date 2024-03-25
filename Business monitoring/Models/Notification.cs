using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class Notification : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Text { get; set; }
    [Required] public UserModel User { get; set; }
}