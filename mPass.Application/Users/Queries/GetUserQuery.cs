using FluentValidation;
using MediatR;
using mPass.Application.Users.Dtos;
using mPass.Domain;

namespace mPass.Application.Users.Queries;

public record GetUserQuery : IRequest<Result<GetUserDto>>
{
    public Guid? Id { get; init; }
    public string? Email { get; init; }
    public string? Username { get; init; }

    public static GetUserQuery ById(Guid id) => new() { Id = id };
    public static GetUserQuery ByEmail(string email) => new() { Email = email };
    public static GetUserQuery ByUsername(string username) => new() { Username = username };

    public bool IsValid()
    {
        var paramCount = new[] { Id.HasValue, !string.IsNullOrEmpty(Email), !string.IsNullOrEmpty(Username) }
            .Count(x => x);
        
        return paramCount == 1;
    }
}