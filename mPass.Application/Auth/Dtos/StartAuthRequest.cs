using System.ComponentModel;

namespace mPass.Application.Auth.Dtos;

public class StartAuthRequest
{
    [Description("Email or Username")] public required string Identifier { get; init; }
}