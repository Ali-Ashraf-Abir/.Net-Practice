using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string getGamesEndpointName = "GetGames";


    public static void MapGamesEndpoints(this WebApplication app)
    {

        // grouping all endpoints
        var group = app.MapGroup("/games");
        // getting all games
        group.MapGet("/", async (
        string? title,
        int? genreId,
        int? page,
        int? pageSize,
        string? sortBy,
        IGameRepository repo
       ) =>

        {
            var result = await repo.GetGamesAsync(title, genreId, page, pageSize, sortBy);
            return Results.Ok(result);
        }
        );

        // getting games by id
        group.MapGet("/{id}", async (int id, IGameRepository repo) =>
        {
            var game = await repo.GetGameByIdAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(game);
        }).WithName(getGamesEndpointName);

        // This endpoint is added to demonstrate how to use the Created result with a location header pointing to the newly created resource.
        group.MapPost("/", async (CreateGameDto game, IGameRepository repo) =>
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
            await repo.CreateGameAsync(newGame);
            return Results.CreatedAtRoute(getGamesEndpointName, new { id = newGame.Id }, newGame);
        });

        group.MapDelete("/{id}", async (int id, IGameRepository repo) =>
        {
            var game = await repo.GetGameByIdAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            await repo.DeleteGameAsync(id);
            return Results.NoContent();
        });

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, IGameRepository repo) =>
        {
            var game = await repo.GetGameByIdAsync(id);
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
            await repo.UpdateGameAsync(game);
            return Results.Ok(game);
        });
    }

}