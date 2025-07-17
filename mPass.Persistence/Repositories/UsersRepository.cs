using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mPass.Domain;
using mPass.Domain.Entities;
using mPass.Domain.Repositories;

namespace mPass.Persistence.Repositories;

public class UsersRepository(MPassDbContext dbContext, ILogger<UsersRepository> logger) : IUsersRepository
{
    private const string EmailInUseErrorMessage = "Email is already in use";
    private const string UsernameInUseErrorMessage = "Username is already in use";

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<Result<User>> CreateAsync(string email, string? username, string salt, string verifier,
        CancellationToken cancellationToken = default)
    {
        var isEmailAvailable = await IsEmailAvailableAsync(email, cancellationToken);
        if (!isEmailAvailable)
        {
            logger.LogDebug("Email {Email} is already in use", email);
            return Result<User>.Failure(EmailInUseErrorMessage);
        }
        var isUsernameAvailable = await IsUsernameAvailableAsync(username, cancellationToken);
        if (!isUsernameAvailable)
        {
            logger.LogDebug("Username {Username} is already in use", username);
            return Result<User>.Failure(UsernameInUseErrorMessage);
        }
        var user = new User
        {
            Email = email,
            Username = username,
            SrpSalt = salt,
            SrpVerifier = verifier
        };
        await dbContext.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User {Id}, {Email} created", user.Id, user.Email);
        return Result<User>.Success(user);
    }
    
    private async Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
    
    private async Task<bool> IsUsernameAvailableAsync(string? username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(username))
        {
            return true;
        }
        return await dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }
}