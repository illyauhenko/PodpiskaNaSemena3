using PodpiskaNaSemena.Application.Models.Seed;

public interface ISeedService
{
    Task<SeedModel> GetByIdAsync(int id, CancellationToken ct = default);
}