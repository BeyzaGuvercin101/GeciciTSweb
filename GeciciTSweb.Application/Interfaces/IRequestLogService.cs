using GeciciTSweb.Application.DTOs;

namespace GeciciTSweb.Application.Interfaces;

public interface IRequestLogService
{
    Task<IEnumerable<RequestLogDto>> GetByRequestIdAsync(int requestId);
    Task<IEnumerable<RequestLogDto>> GetByUserAsync(string keycloakSub);
    Task<int> CreateAsync(CreateRequestLogDto dto, string keycloakSub);
    Task<bool> SoftDeleteAsync(int id, string keycloakSub);
}
