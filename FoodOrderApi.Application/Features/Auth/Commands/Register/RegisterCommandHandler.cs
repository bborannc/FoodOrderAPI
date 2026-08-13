using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Entities;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CustomResponseDto<int>>
    {
        private readonly AppDbContext _context;

        public RegisterCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<int>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. E-posta adresi kontrolü
            var exists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (exists)
            {
                return CustomResponseDto<int>.Fail(400, "Bu e-posta adresi ile zaten kayıtlı bir kullanıcı bulunmaktadır.");
            }

            // 2. Şifreyi HMACSHA512 ile Hash/Salt yapıyoruz
            HashingHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // 3. User Entity nesnesini oluşturuyoruz
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role,
                IsDeleted = false
            };

            // 4. Veritabanına kaydediyoruz
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<int>.Success(201, user.Id);
        }
    }
}