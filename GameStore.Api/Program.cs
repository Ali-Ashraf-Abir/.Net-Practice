
using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
// Adding validation services to the service container.
builder.Services.AddValidation();

var connString = builder.Configuration.GetConnectionString("GameStore");
// Adding the DbContext to the service container with a connection string from the configuration.
if (connString is null)
{
    throw new InvalidOperationException("GameStore connection string is not configured.");
}
builder.AddGameStoreDb(connString);

var app = builder.Build();
app.MigrateDb();

app.MapGamesEndpoints();

app.Run();
