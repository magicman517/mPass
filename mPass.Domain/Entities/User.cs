using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace mPass.Persistence.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User : BaseEntity
{
    [MaxLength(256)] public required string Email { get; set; }
    [MaxLength(100)] public string? Username { get; set; }
    [MaxLength(512)] public required string PasswordHash { get; set; }
}