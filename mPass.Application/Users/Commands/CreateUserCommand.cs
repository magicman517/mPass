using FluentValidation;
using MediatR;
using mPass.Domain;
using mPass.Domain.Entities;

namespace mPass.Application.Users.Commands;

public record CreateUserCommand : IRequest<Result<User>>
{
    public required string Email { get; init; }
    public string? Username { get; init; }
    public required string Salt { get; init; }
    public required string Verifier { get; set; }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();

        When(x => !string.IsNullOrEmpty(x.Username), () =>
        {
            RuleFor(x => x.Username)
                .MinimumLength(2)
                .MaximumLength(128)
                .Must(username =>
                    !new System.ComponentModel.DataAnnotations.EmailAddressAttribute()
                        .IsValid(username)
                )
                .WithMessage("Username cannot be an email address");
        });
        
        RuleFor(x => x.Salt)
            .NotEmpty()
            .Length(32)
            .Matches("^[0-9A-Fa-f]{32}$")
            .WithMessage("Salt must be a 32-character hexadecimal string");
        
        RuleFor(x => x.Verifier)
            .NotEmpty()
            .Length(512)
            .Matches("^[0-9A-Fa-f]{512}$")
            .WithMessage("Verifier must be a 512-character hexadecimal string");
    }
}