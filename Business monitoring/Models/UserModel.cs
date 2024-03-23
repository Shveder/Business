using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class UserModel
{
    [Key] public Guid Id { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Salt { get; set; }
    [Required] public int Role { get; set; }
    [Required] public double Balance { get; set; }
    [Required] public bool IsBlocked { get; set; }
    [Required] public bool IsDeleted { get; set; }

    public UserModel(Guid id, string login, string password, string salt, int role, double balance, bool isBlocked, bool isDeleted)
    {
        Id = id;
        Login = login;
        Password = password;
        Salt = salt;
        Role = role;
        Balance = balance;
        IsBlocked = isBlocked;
        IsDeleted = isDeleted;
    }
}