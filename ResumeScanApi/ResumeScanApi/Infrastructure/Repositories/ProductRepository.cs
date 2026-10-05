using System.Data;
using Dapper;
using ResumeScanApi.Application.Products;
using ResumeScanApi.Domain.Entities;
using ResumeScanApi.Infrastructure.Data;

namespace ResumeScanApi.Infrastructure.Repositories;

public sealed class ProductRepository(ISqlConnectionFactory connectionFactory) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Product_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<Product>(command);
        return rows.AsList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Product_GetById", new { Id = id }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Product>(command);
    }

    public async Task<int> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Product_Create", product, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command);
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Product_Update", product, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition("Master.Product_Delete", new { Id = id }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }
}
