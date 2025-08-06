using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Data;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly GeciciTSwebDbContext _context;
    private readonly IMapper _mapper;

    public UserService(GeciciTSwebDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserListDto>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<UserListDto> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return _mapper.Map<UserListDto>(user);
    }

    public async Task<UserListDto> CreateAsync(CreateUserDto dto)
    {
        var entity = _mapper.Map<User>(dto);
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserListDto>(entity);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

}
