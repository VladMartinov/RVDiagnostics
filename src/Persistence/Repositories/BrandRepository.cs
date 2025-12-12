using System.Diagnostics.CodeAnalysis;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Extensions;

namespace Persistence.Repositories;

public class BrandRepository(DContext context) : IBrandRepository
{
    [SuppressMessage("ReSharper", "EntityFramework.ClientSideDbFunctionCall")]
    public async Task<IEnumerable<Brand>> GetBrandsAsync(string? searchString, int page, int limit, bool track = true,
        CancellationToken cancellationToken = default)
    {
        searchString = searchString?.ToUpperInvariant();
        return await context.Brands
            .ConfigureTracking(track)
            .Where(x => EF.Functions.Like(
                EF.Functions.Collate(x.Name, "NOCASE"),
                $"{searchString}%"
            ))
            .OrderBy(x => x.Name)
            .Skip(page * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<Brand?> GetBrandAsync(string name, bool track = true, CancellationToken cancellationToken = default)
    {
        return await context.Brands.ConfigureTracking(track)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}