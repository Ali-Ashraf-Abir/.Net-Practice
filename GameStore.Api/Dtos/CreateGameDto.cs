using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{

    public record CreateGameDto(
        [Required][StringLength(50)] string Title,
        string Description,
        [Required][Range(0, 1000)] decimal Price,
        DateTime ReleaseDate,
        [Required] string Genre,
        string Developer,
        string Publisher
    );
};