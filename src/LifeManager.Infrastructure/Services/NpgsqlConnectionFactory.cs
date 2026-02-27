using System.Data;
using LifeManager.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LifeManager.Infrastructure.Services;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
