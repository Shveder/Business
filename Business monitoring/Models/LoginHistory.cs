using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Business_monitoring.Models;

public class LoginHistory : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public  IPAddress Ip { get; set; }
    [Required] public UserModel User { get; set; }
}