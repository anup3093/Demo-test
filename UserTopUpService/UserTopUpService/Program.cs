using UserTopUpService.DataBase;
using Swashbuckle.AspNetCore.Swagger;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;
using UserTopUpService.Repositories;
using UserTopUpService.Services;
using UserTopUpService.Validation;
using FluentValidation.AspNetCore;
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});
var config = builder.Configuration;
config.AddEnvironmentVariables("UserTopUpService_");
builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<Program>();
    x.DisableDataAnnotationsValidation = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
  new SqliteConnectionFactory(config.GetValue<string>("Database:ConnectionString")!));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<IBeneficiaryRepository, BeneficiaryRepository>();
builder.Services.AddSingleton<ITopUpHistoryRepository, TopUpHistoryRepository>();
builder.Services.AddSingleton<ITopUpService, TopUpService>();
builder.Services.AddSingleton<IUserBalanceService, UserBalanceService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddHttpClient();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();
app.Run();