using FluentValidation;
using ForumProjects.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumProjects.Infrastructure.FluentValidation
{
    public class AccountDTOValidator : AbstractValidator<AccountCreateDTO>
    {
        public AccountDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("İsim alanı boş olamaz.")
                .MaximumLength(50).WithMessage("İsim 50 karakterden uzun olamaz.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyisim alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Soyisim 50 karakterden uzun olamaz.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Geçerli bir telefon numarası girin.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi alanı boş olamaz.")
                .LessThan(DateTime.Now).WithMessage("Doğum tarihi gelecekte olamaz.");
        }
    }
}
