using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class Card : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public string CardNumber { get; set; }
    [Required] public string Date{ get; set; }
    [Required] public string Cvv { get; set; }
    [Required] public string Name { get; set; }
    [Required] public UserModel User { get; set; }
}