using FluentValidation;
using ForumProjects.Infrastructure.Data;
using ForumProjects.Infrastructure.DTOs;
using ForumProjects.Infrastructure.Entities;
using ForumProjects.Infrastructure.Enums;
using ForumProjects.Infrastructure.FluentValidation;
using ForumProjects.Infrastructure.Repository;
using ForumProjects.Infrastructure.StandardResult;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumProjects.Application.Services
{
    public class UserService<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly GenericRepository<T> _genericRepository;
        private readonly IValidator<AccountCreateDTO> _validationRules;
        private readonly UserManager<IdentityUser> _userManager;


        public UserService(AppDbContext context, GenericRepository<T> genericRepository, IValidator<AccountCreateDTO> validationRules, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _genericRepository = genericRepository;
            _validationRules = validationRules;
            _userManager = userManager;
        }

        public List<AccountDTO> GetAll()
        {
            var accountList = _context.Accounts.ToList();

            var result = accountList.Select(x => new AccountDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                BirthDate = x.BirthDate,
                PhoneNumber = x.PhoneNumber,
                UserLevels = x.UserLevels,
                UserName = x.UserName
            }).ToList();

            var result2 = _genericRepository.GetAll();

            return result;
        }

        public async Task<StandardResult<AccountCreateDTO>> Create(AccountCreateDTO model)
        {
            try
            {
                if (model == null)
                    return new StandardResult<AccountCreateDTO>(false, "Model boş olamaz.");

                var control = _validationRules.Validate(model);
                if (!control.IsValid)
                {
                    var errorMessages = string.Join(", ", control.Errors.Select(e => e.ErrorMessage));
                    return new StandardResult<AccountCreateDTO>(false, errorMessages);
                }

                var userEntity = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(userEntity, model.Password);
                if (!result.Succeeded)
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new StandardResult<AccountCreateDTO>(false, errorMessages);
                }

                var accountEntity = new Account
                {
                    Id = Guid.NewGuid(),
                    UserId = userEntity.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.Password,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserLevels = UserLevel.Member
                };

                await _context.Accounts.AddAsync(accountEntity);
                await _context.SaveChangesAsync();

                return new StandardResult<AccountCreateDTO>(true, "Kullanıcı başarıyla oluşturuldu.", model);
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun bir hata mesajı dönebiliriz
                return new StandardResult<AccountCreateDTO>(false, $"Bir hata oluştu: {ex.Message}");
            }
        }

    }
}
