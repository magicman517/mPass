using FluentAssertions;
using mPass.Domain.Entities;
using NodaTime;

namespace mPass.Domain.Tests.Entities;

public class UsersTests
{
    [Fact]
    public void UserCreation_Should_Set_Properties_Correctly()
    {
        var email = "mail@example.com";
        var username = "testuser";
        var passwordHash = "password123";

        var user = new User
        {
            Email = email,
            Username = username,
            PasswordHash = passwordHash
        };

        user.Id.Should().NotBeEmpty();
        user.Email.Should().Be(email);
        user.Username.Should().Be(username);
        user.PasswordHash.Should().Be(passwordHash);

        var now = SystemClock.Instance.GetCurrentInstant();
        Duration.FromMilliseconds(Math.Abs((user.CreatedAt - now).TotalMilliseconds))
            .Should().BeLessThan(Duration.FromSeconds(1));
        Duration.FromMilliseconds(Math.Abs((user.UpdatedAt - now).TotalMilliseconds))
            .Should().BeLessThan(Duration.FromSeconds(1));
    }

    [Fact]
    public void User_Should_Inherit_From_BaseEntity()
    {
        var user = new User
        {
            Email = "mail@example.com",
            PasswordHash = "password123"
        };

        user.Id.Should().NotBeEmpty();
        user.CreatedAt.Should().NotBe(default);
        user.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public void User_Should_Generate_Unique_Ids()
    {
        var user1 = new User { Email = "testuser1@example.com", PasswordHash = "password123" };
        var user2 = new User { Email = "testuser2@example.com", PasswordHash = "password123" };

        user1.Id.Should().NotBe(user2.Id);
    }

    [Theory]
    [InlineData("user1@example.com", "username", "password123")]
    [InlineData("another@test.com", null, "password456")]
    [InlineData("third@domain.org", "", "password789")]
    public void User_Should_Accept_Various_Valid_Inputs(string email, string? username, string passwordHash)
    {
        var user = new User
        {
            Email = email,
            Username = username,
            PasswordHash = passwordHash
        };

        user.Email.Should().Be(email);
        user.Username.Should().Be(username);
        user.PasswordHash.Should().Be(passwordHash);
    }
    
    [Fact]
    public void User_Should_Allow_Null_Username()
    {
        var user = new User
        {
            Email = "test@example.com",
            Username = null,
            PasswordHash = "password123"
        };

        user.Username.Should().BeNull();
    }
}
