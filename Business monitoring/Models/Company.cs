﻿using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class Company : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Salt { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Phone { get; set; }
    [Required] public bool IsBlocked { get; set; }
    [Required] public bool IsDeleted { get; set; }
    
}
