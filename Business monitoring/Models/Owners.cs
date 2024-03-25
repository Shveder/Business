using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class Owners : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public int NumberOfShares { get; set; }
    [Required] public UserModel User { get; set; }
    [Required] public Business Business { get; set; }
}