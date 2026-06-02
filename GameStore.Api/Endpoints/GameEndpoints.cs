using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string getGamesEndpointName = "GetGames";
    private static readonly List<GameDto> games =
    [
        new (1, "The Legend of Zelda: Breath of the Wild", "An open-world action-adventure game set in the kingdom of Hyrule.", 59.99m, new DateTime(2017, 3, 3), "Action-Adventure", "Nintendo", "Nintendo"),
        new (2, "God of War", "An action-adventure game following Kratos and his son Atreus on a journey through Norse mythology.", 49.99m, new DateTime(2018, 4, 20), "Action-Adventure", "Santa Monica Studio", "Sony Interactive Entertainment"),
        new (3, "Red Dead Redemption 2", "An open-world action-adventure game set in the American Wild West.", 59.99m, new DateTime(2018, 10, 26), "Action-Adventure", "Rockstar Games", "Rockstar Games"),
        new (4, "The Witcher 3: Wild Hunt", "An open-world action RPG following Geralt of Rivia on his quest to find his adopted daughter.", 39.99m, new DateTime(2015, 5, 19), "Action RPG", "CD Projekt Red", "CD Projekt"),
        new (5, "Minecraft", "A sandbox game that allows players to build and explore virtual worlds made of blocks.", 26.95m, new DateTime(2011, 11, 18), "Sandbox", "Mojang Studios", "Mojang Studios")
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {

        // grouping all endpoints
        var group = app.MapGroup("/games");

        // getting all games
        group.MapGet("/", (GameStoreContext dbContext) => dbContext.Games.ToList());

        // getting games by id
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.Find(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(game);
        }).WithName(getGamesEndpointName);

        // This endpoint is added to demonstrate how to use the Created result with a location header pointing to the newly created resource.
        group.MapPost("/", async(CreateGameDto game, GameStoreContext dbContext) =>
        {
            Game newGame = new()
            {
              Title = game.Title,
              Description = game.Description,
              Price = game.Price,
              ReleaseDate = game.ReleaseDate,
              GenreId = game.GenreId,
              Developer = game.Developer,
              Publisher = game.Publisher
            };
            dbContext.Games.Add(newGame);
            await dbContext.SaveChangesAsync();
            return Results.CreatedAtRoute(getGamesEndpointName,new {id=newGame.Id},newGame);
        });

        group.MapDelete("/{id}", async(int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.Find(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            dbContext.Games.Remove(game);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.Find(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            game.Title = updatedGame.Title;
            game.Description = updatedGame.Description;
            game.Price = updatedGame.Price;
            game.ReleaseDate = updatedGame.ReleaseDate;
            game.GenreId = updatedGame.GenreId;
            game.Developer = updatedGame.Developer;
            game.Publisher = updatedGame.Publisher;
            await dbContext.SaveChangesAsync();
            return Results.Ok(game);
        });
    }

}