using Bogus;

namespace Persistence.Tests.Extensions;

public static class FakerExtensions
{
    /// <summary>
    /// Get a random int from 1 to sideCount.
    /// </summary>
    /// <param name="faker"></param>
    /// <param name="sideCount"></param>
    /// <returns></returns>
    public static int GetRandomSide(this Faker faker, int sideCount) => faker.Random.Number(1, sideCount);
}