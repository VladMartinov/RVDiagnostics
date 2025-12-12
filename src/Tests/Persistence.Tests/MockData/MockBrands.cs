using Bogus;
using Core.Entities;
using Persistence.Contexts;
using Persistence.Tests.Extensions;

namespace Persistence.Tests.MockData;

public static class MockBrands
{
    public static List<Brand> GenerateBrand(int count)
    {
        var f = new Faker<Brand>()
            .RuleFor(x => x.Name, f =>  f.Company.CompanyName() + "_" + f.UniqueIndex)
            .RuleFor(x => x.LogoPath, f => f.Image.PicsumUrl(200, 200))
            .RuleFor(x => x.Description, f => f.Random.Bool() ? f.Lorem.Paragraph() : null);

        return f.Generate(count);
    }
    
    public static async Task<List<Brand>> CreateBrands(this DContext context, int count)
    {
        var brands = GenerateBrand(count);
        await context.AddRangeAsync(brands);
        await context.SaveChangesAsync();
        return brands;
    }
}