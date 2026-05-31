namespace GameStore.Api.Dtos
{

    public record CreateGameDto(
        string Title,
        string Description,
        decimal Price,
        DateTime ReleaseDate,
        string Genre,
        string Developer,
        string Publisher
    );
};