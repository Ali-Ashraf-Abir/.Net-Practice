using GameStore.Api.Dtos;
using GameStore.Api.Models;

namespace GameStore.Api.Repositories;

public interface IGenreRepository
{
    Task<Genre>  CreateGenreAsync(Genre genre);
    Task<IEnumerable<Genre>> GetGenresAsync();
    Task<Genre?> GetGenreByIdAsync(int id);
    Task<Genre?> UpdateGenreAsync(Genre genre);
    Task DeleteGenreAsync(int id);
}