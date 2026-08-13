using FluentValidation;

namespace FoodOrderApi.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Ad soyad alanı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.")
                .MinimumLength(2).WithMessage("Ad soyad en az 2 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(150).WithMessage("E-posta adresi en fazla 150 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Geçersiz kullanıcı rolü seçimi.");
        }
    }
}
