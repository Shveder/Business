using System.ComponentModel.DataAnnotations;

 namespace Business_monitoring.Models;
 
 public class RecentPasswords : BaseEntity
 {
     [Key] public Guid Id { get; set; }
     [Required] public string Password { get; set; }
     [Required] public UserModel User { get; set; }
 }