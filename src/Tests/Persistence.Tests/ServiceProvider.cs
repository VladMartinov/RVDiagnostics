using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence.Tests;

public class ServiceProvider : IAsyncDisposable
{
    private DbContextOptions<DContext>? _options;
    private SqliteConnection? _connection;
    private IServiceProvider? _serviceProvider;
    private bool _isInitialized;
    
    
    public async Task<IServiceProvider> GetServiceProvider()
    {
        if (_isInitialized) return _serviceProvider!;
        var sc = new ServiceCollection();
        await Initialize(sc);

        sc.AddPersistence();
        
        _serviceProvider = sc.BuildServiceProvider();
        return _serviceProvider;
    }

    private async Task Initialize(IServiceCollection services)
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<DContext>()
            .UseSqlite(_connection)
            .Options;

        await using var context = new DContext(_options);
        await context.Database.EnsureCreatedAsync();
        services.AddDbContext<DContext>(s => s.UseSqlite(_connection));
        _isInitialized = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
        
        GC.SuppressFinalize(this);
    }
}