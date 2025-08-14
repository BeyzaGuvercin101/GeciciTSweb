using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Helpers;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using GeciciTSweb.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var data = await _unitOfWork.Users.FirstOrDefaultAsync(c=>c.Username == request.Username && !c.IsDeleted);

            if(data == null)
            {
                throw new InvalidOperationException($"User not found");
            }

            var check = CryptHelper.Verify(request.Password, data.Password);

            if (check) 
            {
                return new LoginResponse() 
                { 
                    Succeeded = true, 
                    Token = "1"
                };
            }

            return new LoginResponse()
            {
                Succeeded = false
            };
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var data = await _unitOfWork.Users.FirstOrDefaultAsync(c => c.Username == request.Username);

            if (data != null)
            {
                throw new InvalidOperationException($"User already exists!");
            }

            var entity = _mapper.Map<User>(request);

            //Parolayı hashle 
            string hashedPassword = CryptHelper.HashPassword(request.Password);

            entity.Password = hashedPassword;

            var result = await _unitOfWork.Users.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            if (result.Id > 0) 
            {
                return new RegisterResponse() { Succeeded = true };
            }

            return new RegisterResponse() { Succeeded = false };
        }
    }
}
