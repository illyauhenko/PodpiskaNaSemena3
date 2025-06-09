using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Seed
{
    public sealed record SeedModel(
        int Id,
        string Username,
        string Name,
        string Description,
        decimal Price
    ) : Model(Id, Username);
}