namespace ResumeScanApi.Application.Products;

public sealed record ProductDto(int Id, string Name, decimal Price, int Quantity);
public sealed record CreateProductRequest(string Name, decimal Price, int Quantity);
public sealed record UpdateProductRequest(string Name, decimal Price, int Quantity);
