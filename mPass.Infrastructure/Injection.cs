using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mPass.Domain.Repositories;
using mPass.Persistence;
using mPass.Persistence.Repositories;
using StackExchange.Redis;

namespace mPass.Infrastructure;

public static class Injection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<MPassDbContext>(opt =>
            opt.UseNpgsql(
                configuration.GetConnectionString("Postgres"),
                o => o.UseNodaTime()
            ));
        
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
        services.AddScoped<IDatabase>(provider =>
            provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}