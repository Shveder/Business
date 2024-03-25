using System.ComponentModel.DataAnnotations;
using System.Net;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class LoginHistory : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public  IPAddress Ip { get; set; }
    [Required] public UserModel User { get; set; }
}