namespace mPass.Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public string? Username { get; set; }
    public required string SrpSalt { get; set; }
    public required string SrpVerifier { get; set; }
}