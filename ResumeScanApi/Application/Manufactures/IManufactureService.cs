using ResumeScanApi.Application.Contracts;

namespace ResumeScanApi.Application.Manufactures;

public interface IManufactureService
{
    Task<ApiResponse<IReadOnlyList<ManufactureDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<ManufactureDto>> GetByIdAsync(int manufactureId, CancellationToken cancellationToken);
    Task<ApiResponse<ManufactureDto>> CreateAsync(CreateManufactureRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<ManufactureDto>> UpdateAsync(int manufactureId, UpdateManufactureRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteAsync(int manufactureId, CancellationToken cancellationToken);
}
