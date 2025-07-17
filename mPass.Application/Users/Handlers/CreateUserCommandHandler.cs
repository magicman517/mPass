using MediatR;
using mPass.Application.Users.Commands;
using mPass.Domain;
using mPass.Domain.Entities;
using mPass.Domain.Repositories;

namespace mPass.Application.Users.Handlers;

public class CreateUserCommandHandler(IUsersRepository usersRepository)
    : IRequestHandler<CreateUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await usersRepository.CreateAsync(request.Email, request.Username, request.Salt, request.Verifier,
            cancellationToken);
        return result.IsSuccess
            ? Result<User>.Success(result.Value)
            : Result<User>.Failure(result.Errors);
    }
}