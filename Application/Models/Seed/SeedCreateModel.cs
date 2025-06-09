
namespace PodpiskaNaSemena.Application.Models.Seed
{
    public sealed record SeedCreateModel(
        string Name,
        string Description,
        decimal Price
    );
}