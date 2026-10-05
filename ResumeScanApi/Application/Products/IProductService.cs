using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.Products;

public interface IProductService
{
    Task<ApiResponse<IReadOnlyList<ProductDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<ProductDto>> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken);
}
