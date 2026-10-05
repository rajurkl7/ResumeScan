using AutoMapper;
using ResumeScanApi.Application.Contracts;
using ResumeScanApi.Domain.Entities;

namespace ResumeScanApi.Application.Products;

public sealed class ProductService(IProductRepository repository, IMapper mapper) : IProductService
{
    public async Task<ApiResponse<IReadOnlyList<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return ApiResponse<IReadOnlyList<ProductDto>>.Ok(mapper.Map<IReadOnlyList<ProductDto>>(products));
    }

    public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);
        return product is null
            ? ApiResponse<ProductDto>.Fail("Product was not found.")
            : ApiResponse<ProductDto>.Ok(mapper.Map<ProductDto>(product));
    }

    public async Task<ApiResponse<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product.Id = await repository.CreateAsync(product, cancellationToken);
        return ApiResponse<ProductDto>.Ok(mapper.Map<ProductDto>(product), "Product created successfully.");
    }

    public async Task<ApiResponse<ProductDto>> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product.Id = id;
        return await repository.UpdateAsync(product, cancellationToken)
            ? ApiResponse<ProductDto>.Ok(mapper.Map<ProductDto>(product), "Product updated successfully.")
            : ApiResponse<ProductDto>.Fail("Product was not found.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(id, cancellationToken)
            ? ApiResponse<bool>.Ok(true, "Product deleted successfully.")
            : ApiResponse<bool>.Fail("Product was not found.");
    }
}
