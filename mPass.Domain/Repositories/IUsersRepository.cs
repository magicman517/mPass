using mPass.Domain.Entities;

namespace mPass.Domain.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Result<User>> CreateAsync(string email, string? username, string salt, string verifier, CancellationToken cancellationToken = default);
}