using mPass.Domain.Entities;

namespace mPass.Application.Users.Dtos;

public class GetUserDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public string? Username { get; set; }
    public required string SrpSalt { get; set; }
    public required string SrpVerifier { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static GetUserDto MapDoDto(User user)
    {
        return new GetUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            SrpSalt = user.SrpSalt,
            SrpVerifier = user.SrpVerifier,
            CreatedAt = user.CreatedAt.ToDateTimeUtc(),
            UpdatedAt = user.UpdatedAt.ToDateTimeUtc()
        };
    }
}