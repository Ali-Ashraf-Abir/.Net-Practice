namespace GameStore.Api.Dtos;

public record GameDto(
    int Id,
    string Title,
    string Description,
    decimal Price,
    DateTime ReleaseDate,
    string Genre,
    string Developer,
    string Publisher
);