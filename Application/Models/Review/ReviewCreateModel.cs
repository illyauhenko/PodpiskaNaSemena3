namespace PodpiskaNaSemena.Application.Models.Review
{
    public sealed record ReviewCreateModel(
        int UserId,
        int SeedId,
        int Rating,
        string? Comment
    );
}