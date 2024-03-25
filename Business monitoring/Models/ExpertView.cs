using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class ExpertView : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public string View { get; set; }
    [Required] public Business Business { get; set; }
    [Required] public Expert Expert { get; set; }
}