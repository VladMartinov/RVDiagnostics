using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Tests.MockData;

namespace Persistence.Tests.RepositoryTests.BrandRepository;

public class GetBrandsTests : IAsyncLifetime
{
    private ServiceProvider _provider = null!;
    private IServiceProvider _serviceProvider = null!;
    private IBrandRepository _brandRepository = null!;
    private DContext _context = null!;
    private readonly List<Brand> _brands = [];
    
    public async Task InitializeAsync()
    {
        _provider = new ServiceProvider();
        _serviceProvider = await _provider.GetServiceProvider();
        _brandRepository = _serviceProvider.GetRequiredService<IBrandRepository>();
        _context = _serviceProvider.GetRequiredService<DContext>();

        var brands = await _context.CreateBrands(1000);
        _brands.AddRange(brands.OrderBy(x => x.Name));
    }

    public async Task DisposeAsync()
    {
        await _provider.DisposeAsync();
    }
    
    [Fact]
    public async Task GetBrands_ShouldReturnAllBrands()
    {
        // Act
        var brands = (await _brandRepository.GetBrandsAsync("",0, 2000, false))
            .ToList();

        // Assert
        Assert.NotNull(brands);
        Assert.Equal(1000, brands.Count);
    }

    [Fact]
    public async Task GetBrands_ShouldReturnNothing()
    {
        // Act
        var brands = await _brandRepository.GetBrandsAsync("",0, 0, false);
        
        // Assert
        Assert.NotNull(brands);
        Assert.Empty(brands);
    }

    [Fact]
    public async Task GetBrands_WithDifferentCase_ReturnsAllMatching()
    {
        var searchTerm = _brands.First().Name.ToLower()[..3];
        
        var foundBrands = (await _brandRepository.GetBrandsAsync(searchTerm,0, 1000, false))
            .ToList();
        
        var matchingBrands = _brands
            .Where(x => x.Name.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
        
        Assert.Equal(matchingBrands.Count, foundBrands.Count);
    }
}