using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;
 
 namespace Business_monitoring.Models;
 
 public class RecentPasswords : IModels
 {
     [Key] public Guid Id { get; set; }
     [Required] public string Password { get; set; }
     [Required] public UserModel User { get; set; }
 }