using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;


public class GameRepository(GameStoreContext dbContext) : IGameRepository
{
    public async Task<PagedResult<Game>> GetGamesAsync(string? title,
    int? genreId,
    int? page,
    int? pageSize,
    string? sortBy)
    {
        var query = dbContext.Games.Include(g => g.Genre).AsQueryable();
        if (!string.IsNullOrWhiteSpace(title))
        {
            title = title.ToLower();
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
        return new PagedResult<Game>
        {
            Items = games,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<Game?> GetGameByIdAsync(int id)
    {
        var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return null;
            }
            else
            {
                return game;
            }
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        return game;
    }

    public async Task UpdateGameAsync(Game game)
    {
        dbContext.Games.Update(game);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteGameAsync(int id)
    {
        var game = await dbContext.Games.FindAsync(id);
        if (game != null)
        {
            dbContext.Games.Remove(game);
            await dbContext.SaveChangesAsync();
        }
    }
}