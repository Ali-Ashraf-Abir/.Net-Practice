using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;


public class GenreRepository(GameStoreContext dbContext) : IGenreRepository
{
    public async Task<Genre>  CreateGenreAsync(Genre genre)
    {

        dbContext.Genres.Add(genre);
        await dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task DeleteGenreAsync(int id)
    {
        var genre = await GetGenreByIdAsync(id);
        if (genre != null)
        {
            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Genre?> GetGenreByIdAsync(int id)
    {
        return await dbContext.Genres.FindAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetGenresAsync()
    {
        return await dbContext.Genres.ToListAsync();
    }


    public async Task<Genre?> UpdateGenreAsync(Genre genre)
    {
        var existingGenre = await GetGenreByIdAsync(genre.Id);
        if (existingGenre == null)
        {
            return null;
        }

        dbContext.Entry(existingGenre).CurrentValues.SetValues(genre);
        await dbContext.SaveChangesAsync();
        return existingGenre;
    }


}