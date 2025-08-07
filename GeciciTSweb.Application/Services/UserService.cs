using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserListDto>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.FindAsync(u => !u.IsDeleted);
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<UserListDto> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        return _mapper.Map<UserListDto>(user);
    }

    public async Task<UserListDto> CreateAsync(CreateUserDto dto)
    {
        var entity = _mapper.Map<User>(dto);
        await _unitOfWork.Users.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserListDto>(entity);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null || user.IsDeleted) return false;

        user.IsDeleted = true;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

}
