

using GameStore.Api.Dtos;
using GameStore.Api.Models;
namespace GameStore.Api.Repositories;

public interface IGameRepository
{
    Task<PagedResult<Game>> GetGamesAsync(
    string? title, 
    int? genreId, 
    int? page, 
    int? pageSize, 
    string? sortBy);
    Task<Game?> GetGameByIdAsync(int id);
    Task<Game> CreateGameAsync(Game game);
    Task UpdateGameAsync(Game game);
    Task DeleteGameAsync(int id);
}