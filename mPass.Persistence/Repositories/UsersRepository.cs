using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mPass.Domain;
using mPass.Domain.Entities;
using mPass.Domain.Repositories;

namespace mPass.Persistence.Repositories;

public class UsersRepository(MPassDbContext dbContext, ILogger<UsersRepository> logger) : IUsersRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<Result<User>> CreateAsync(string email, string? username, string password, CancellationToken cancellationToken)
    {
        var existingUser = await GetByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            return Result<User>.Failure("Email is already in use");
        }
        var user = new User
        {
            Email = email,
            Username = username,
            PasswordHash = password // TODO
        };
        await dbContext.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User {Id}, {Email} created", user.Id, user.Email);
        return Result<User>.Success(user);
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}