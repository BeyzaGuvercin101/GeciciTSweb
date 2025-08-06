using GeciciTSweb.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListDto>> GetAllAsync();
        Task<UserListDto> GetByIdAsync(int id);
        Task<UserListDto> CreateAsync(CreateUserDto dto);
        Task<bool> SoftDeleteAsync(int id);

    }
}
