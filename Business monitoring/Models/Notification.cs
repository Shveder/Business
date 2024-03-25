using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class Notification : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public string Text { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public UserModel User { get; set; }
}