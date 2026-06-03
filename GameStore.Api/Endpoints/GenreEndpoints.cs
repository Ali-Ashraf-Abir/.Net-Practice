using GameStore.Api.Dtos;
using GameStore.Api.Models;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this WebApplication app)
    {
        const string GetGenres = "GetGenres";
        const string GetGenreById = "GetGenreById";
        var mapGroup = app.MapGroup("/genres").WithTags("Genres");
        mapGroup.MapGet("/", async (IGenreRepository repo) =>
        {
            var genres = await repo.GetGenresAsync();
            return Results.Ok(genres);
        }).WithName(GetGenres);

        mapGroup.MapGet("/{id}", async (int id, IGenreRepository repo) =>
        {
            var genre = await repo.GetGenreByIdAsync(id);
            return genre is not null ? Results.Ok(genre) : Results.NotFound();
        }).WithName(GetGenreById);

        mapGroup.MapPost("/", async (CreateGenreDto dto, IGenreRepository repo) =>
        {
            var genre = new Genre { Name = dto.Name };
            var createdGenre = await repo.CreateGenreAsync(genre);
            return Results.CreatedAtRoute(GetGenreById, new { id = createdGenre.Id }, createdGenre);
        });
        mapGroup.MapPut("/{id}", async (int id, CreateGenreDto dto, IGenreRepository repo) =>
        {
            var existingGenre = await repo.GetGenreByIdAsync(id);
            if (existingGenre == null)
            {
                return Results.NotFound();
            }
            existingGenre.Name = dto.Name;
            var updatedGenre = await repo.UpdateGenreAsync(existingGenre);
            return Results.Ok(updatedGenre);
        });
        mapGroup.MapDelete("/{id}", async (int id, IGenreRepository repo) =>
        {
            var existingGenre = await repo.GetGenreByIdAsync(id);
            if (existingGenre == null)
            {
                return Results.NotFound();
            }
            await repo.DeleteGenreAsync(id);
            return Results.NoContent();
        });
    }

}