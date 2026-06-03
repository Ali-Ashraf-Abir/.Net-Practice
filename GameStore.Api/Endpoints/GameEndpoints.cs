using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
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
        GameStoreContext dbContext) =>

        {
            var query = dbContext.Games.Include(g => g.Genre).AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
            {

                query = query.Where(g => g.Title.ToLower().Contains(title));
            }
            if (genreId.HasValue)
            {
                query = query.Where(g => g.GenreId == genreId.Value);
            }




            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "title" => query.OrderBy(g => g.Title),
                    "releasedate" => query.OrderBy(g => g.ReleaseDate),
                    "price" => query.OrderBy(g => g.Price),
                    _ => query
                };
            }
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)(pageSize ?? 10));

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            var games = await query.ToListAsync();
            return Results.Ok(new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Games = games
            });
        }
        );

        // getting games by id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(game);
        }).WithName(getGamesEndpointName);

        // This endpoint is added to demonstrate how to use the Created result with a location header pointing to the newly created resource.
        group.MapPost("/", async (CreateGameDto game, GameStoreContext dbContext) =>
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
            return Results.CreatedAtRoute(getGamesEndpointName, new { id = newGame.Id }, newGame);
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
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