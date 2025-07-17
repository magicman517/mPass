using mPass.API.Middleware;
using mPass.Application;
using mPass.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("reference", opts =>
    {
        opts
            .WithTheme(ScalarTheme.Alternate)
            .WithModels(false)
            .WithClientButton(false);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseVersionHeader();
app.MapControllers();

app.Run();