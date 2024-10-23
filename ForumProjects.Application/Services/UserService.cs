using FluentValidation;
using ForumProjects.Infrastructure.Data;
using ForumProjects.Infrastructure.DTOs;
using ForumProjects.Infrastructure.DTOs.AccountDTOs;
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
    public class UserService
    {
        private readonly AppDbContext _context;
        //private readonly GenericRepository<T> _genericRepository;
        private readonly IValidator<AccountCreateDTO> _validationRules;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserService(AppDbContext context, /*GenericRepository<T> genericRepository, */IValidator<AccountCreateDTO> validationRules, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            //_genericRepository = genericRepository;
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

            //var result2 = _genericRepository.GetAll();

            return result;
        }
        public Account GetById(string Id)
        {
            try
            {
                var userInfo = _context.Accounts.Where(x => x.Id == Id).FirstOrDefault();

                if (userInfo == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                return userInfo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Bir hata oluştu: {ex.Message}", ex);
            }
        }
        public async Task<StandardResult<AccountCreateDTO>> UserCreate(AccountCreateDTO model)
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

                var userEntity = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserLevels = model.UserLevel,
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
                    Id = Guid.NewGuid().ToString(),
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
        public async Task<StandardResult<AccountCreateDTO>> UserUpdate(AccountCreateDTO model)
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

                var userData = _context.Users.Where(x => x.Id == model.Id).FirstOrDefault();

                if (userData == null)
                    return new StandardResult<AccountCreateDTO>(false, "Kullanıcı bulunamadı.", model);

                userData.Email = model.Email;
                userData.LastName = model.LastName;
                userData.FirstName = model.FirstName;
                userData.PhoneNumber = model.PhoneNumber;
                userData.UserName = model.UserName;

                var userChangePassword = await _userManager.ChangePasswordAsync(userData, model.PreviousPassword, model.Password);

                var result = await _userManager.UpdateAsync(userData);
                if (!result.Succeeded)
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new StandardResult<AccountCreateDTO>(false, errorMessages);
                }

                var accountData = _context.Accounts.Where(x => x.UserId == userData.Id).FirstOrDefault();

                if (accountData == null)
                    return new StandardResult<AccountCreateDTO>(false, "Kullanıcı bulunamadı.", model);

                accountData.FirstName = model.FirstName;
                accountData.LastName = model.LastName;
                accountData.BirthDate = model.BirthDate;
                accountData.Email = model.Email;
                accountData.Password = model.Password;
                accountData.ConfirmPassword = model.Password;
                accountData.UserName = model.Email;
                accountData.PhoneNumber = model.PhoneNumber;
                accountData.UserLevels = UserLevel.Member;

                _context.Accounts.Update(accountData);
                _context.SaveChanges();

                return new StandardResult<AccountCreateDTO>(true, "Kullanıcı başarıyla güncellendi.", model);
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun bir hata mesajı dönebiliriz
                return new StandardResult<AccountCreateDTO>(false, $"Bir hata oluştu: {ex.Message}");
            }
        }
        public StandardResult<AccountCreateDTO> UserDelete(string Id)
        {
            try
            {
                var userData = _context.Users.Where(x => x.Id == Id).FirstOrDefault();

                _context.Users.Remove(userData);

                var accountData = _context.Accounts.Where(x => x.UserId == userData.Id).FirstOrDefault();

                _context.Accounts.Remove(accountData);
                _context.SaveChanges();

                return new StandardResult<AccountCreateDTO>(true, "Kullanıcı başarıyla silindi.");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
