using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, CustomResponseDto<TokenDto>>
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<CustomResponseDto<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcıyı buluyoruz
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            // Güvenlik İlkesi: "E-posta veya şifre hatalı" mesajı verilerek hangi bilginin yanlış olduğu ifşa edilmez
            if (user == null)
            {
                return CustomResponseDto<TokenDto>.Fail(400, "E-posta adresi veya şifre hatalı.");
            }

            // 2. Şifre doğrulaması yapıyoruz
            var isPasswordValid = HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                return CustomResponseDto<TokenDto>.Fail(400, "E-posta adresi veya şifre hatalı.");
            }

            // 3. JWT Token ve RefreshToken üretiyoruz
            var tokenDto = _tokenService.CreateToken(user);
            /*
            // 4. Refresh Token bilgilerini veritabanına kaydediyoruz
            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenEndDate = DateTime.UtcNow.AddDays(7); // Refresh token 7 gün geçerli
            */
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<TokenDto>.Success(200, tokenDto);
        }
    }
}