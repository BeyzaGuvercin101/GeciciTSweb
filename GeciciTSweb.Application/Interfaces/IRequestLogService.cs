using GeciciTSweb.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IRequestLogService
    {
        Task<IEnumerable<RequestLogDto>> GetByRequestIdAsync(int requestId);
        Task<int> CreateAsync(CreateRequestLogDto dto);
    }
}
