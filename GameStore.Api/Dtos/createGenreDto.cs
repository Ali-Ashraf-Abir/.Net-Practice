using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public class CreateGenreDto
{
    [Required]
    [MaxLength(15)]
    public string Name { get; set; } = string.Empty;
}