using FluentAssertions;
using mPass.Domain.Entities;
using NodaTime;

namespace mPass.Domain.Tests.Entities;

public class UsersTests
{
    private const string SampleSalt = "A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6";
    private static readonly string SampleVerifier = new string('B', 512);

    [Fact]
    public void UserCreation_Should_Set_Properties_Correctly()
    {
        var email = "mail@example.com";
        var username = "testuser";
        var salt = SampleSalt;
        var verifier = SampleVerifier;

        var user = new User
        {
            Email = email,
            Username = username,
            Salt = salt,
            Verifier = verifier
        };

        user.Id.Should().NotBeEmpty();
        var now = SystemClock.Instance.GetCurrentInstant();
        Duration.FromMilliseconds(Math.Abs((user.CreatedAt - now).TotalMilliseconds))
            .Should().BeLessThan(Duration.FromSeconds(1));
        Duration.FromMilliseconds(Math.Abs((user.UpdatedAt - now).TotalMilliseconds))
            .Should().BeLessThan(Duration.FromSeconds(1));

        user.Email.Should().Be(email);
        user.Username.Should().Be(username);
        user.Salt.Should().Be(salt);
        user.Verifier.Should().Be(verifier);
    }

    [Fact]
    public void User_Should_Inherit_From_BaseEntity()
    {
        var user = new User
        {
            Email = "mail@example.com",
            Salt = SampleSalt,
            Verifier = SampleVerifier
        };

        user.Id.Should().NotBeEmpty();
        user.CreatedAt.Should().NotBe(default(Instant));
        user.UpdatedAt.Should().NotBe(default(Instant));
    }

    [Fact]
    public void User_Should_Generate_Unique_Ids()
    {
        var user1 = new User
        {
            Email = "test1@example.com",
            Salt = SampleSalt,
            Verifier = SampleVerifier
        };
        var user2 = new User
        {
            Email = "test2@example.com",
            Salt = SampleSalt,
            Verifier = SampleVerifier
        };

        user1.Id.Should().NotBe(user2.Id);
    }

    [Theory]
    [InlineData("user1@example.com", "username")]
    [InlineData("another@test.com", null)]
    [InlineData("third@domain.org", "")]
    public void User_Should_Accept_Various_Valid_Inputs(string email, string? username)
    {
        var salt = SampleSalt;
        var verifier = SampleVerifier;

        var user = new User
        {
            Email = email,
            Username = username,
            Salt = salt,
            Verifier = verifier
        };

        user.Email.Should().Be(email);
        user.Username.Should().Be(username);
        user.Salt.Should().Be(salt);
        user.Verifier.Should().Be(verifier);
    }

    [Fact]
    public void User_Should_Allow_Null_Username()
    {
        var user = new User
        {
            Email = "test@example.com",
            Username = null,
            Salt = SampleSalt,
            Verifier = SampleVerifier
        };

        user.Username.Should().BeNull();
    }
}