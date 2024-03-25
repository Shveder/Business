using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class Deposits : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public double SumOfDeposit { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public UserModel User { get; set; }
}