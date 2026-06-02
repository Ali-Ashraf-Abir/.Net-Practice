using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{

    public record UpdateGameDto(
        [Required][StringLength(50)] string Title,
        string Description,
        [Required][Range(0, 1000)] decimal Price,
        DateTime ReleaseDate,
        int GenreId,
        string Developer,
        string Publisher
    );
};