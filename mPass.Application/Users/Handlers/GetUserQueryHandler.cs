using MediatR;
using mPass.Application.Users.Dtos;
using mPass.Application.Users.Queries;
using mPass.Domain;
using mPass.Domain.Entities;
using mPass.Domain.Repositories;

namespace mPass.Application.Users.Handlers;

public class GetUserQueryHandler(IUsersRepository usersRepository) : IRequestHandler<GetUserQuery, Result<GetUserDto>>
{
    public async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
        {
            return Result<GetUserDto>.Failure("Internal error: Invalid request parameters");
        }

        User? user = null;

        if (request.Id.HasValue)
        {
            user = await usersRepository.GetByIdAsync(request.Id.Value, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            user = await usersRepository.GetByEmailAsync(request.Email, cancellationToken);
        }
        
        if (!string.IsNullOrEmpty(request.Username))
        {
            user = await usersRepository.GetByUsernameAsync(request.Username, cancellationToken);
        }
        
        return user is null
            ? Result<GetUserDto>.Failure("User not found")
            : Result<GetUserDto>.Success(GetUserDto.MapDoDto(user));
    }
}