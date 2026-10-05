using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Products;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
