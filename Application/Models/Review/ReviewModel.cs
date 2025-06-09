using PodpiskaNaSemena.Application.Models.Base;

namespace PodpiskaNaSemena.Application.Models.Review
{
    public sealed record ReviewModel(
        int Id,
        string Username,
        string SeedName,
        int Rating,
        string? Comment,
        DateTime CreatedAt
    ) : Model(Id, Username);
}