using Core.Entities;

namespace Core.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetBrandsAsync(string? searchString, int page, int limit, bool track = true, 
        CancellationToken cancellationToken = default);

    Task<Brand?> GetBrandAsync(string name, bool track = true, CancellationToken cancellationToken = default);
}