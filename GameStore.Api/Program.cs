using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new (1, "The Legend of Zelda: Breath of the Wild", "An open-world action-adventure game set in the kingdom of Hyrule.", 59.99m, new DateTime(2017, 3, 3), "Action-Adventure", "Nintendo", "Nintendo"),
    new (2, "God of War", "An action-adventure game following Kratos and his son Atreus on a journey through Norse mythology.", 49.99m, new DateTime(2018, 4, 20), "Action-Adventure", "Santa Monica Studio", "Sony Interactive Entertainment"),
    new (3, "Red Dead Redemption 2", "An open-world action-adventure game set in the American Wild West.", 59.99m, new DateTime(2018, 10, 26), "Action-Adventure", "Rockstar Games", "Rockstar Games"),
    new (4, "The Witcher 3: Wild Hunt", "An open-world action RPG following Geralt of Rivia on his quest to find his adopted daughter.", 39.99m, new DateTime(2015, 5, 19), "Action RPG", "CD Projekt Red", "CD Projekt"),
    new (5, "Minecraft", "A sandbox game that allows players to build and explore virtual worlds made of blocks.", 26.95m, new DateTime(2011, 11, 18), "Sandbox", "Mojang Studios", "Mojang Studios")
];

app.MapGet("/games", () => games);

app.MapPost("/games", (GameDto game) =>
{
    int newId = games.Max(g => g.Id) + 1;
    GameDto newGame = game with { Id = newId };
    games.Add(newGame);
    return Results.Created($"/games/{newId}", newGame);
});

app.MapDelete("/games/{id}", (int id) =>
{
    var game = games.FirstOrDefault(g => g.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    games.Remove(game);
    return Results.NoContent();
});

app.MapPut("/games/{id}", (int id, GameDto updatedGame) =>
{
    var game = games.FirstOrDefault(g => g.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    GameDto newGame = updatedGame with { Id = id };
    int index = games.IndexOf(game);
    games[index] = newGame;
    return Results.Ok(newGame);
});

app.Run();
