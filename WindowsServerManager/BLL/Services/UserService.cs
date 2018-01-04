using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Infrastructure;
using BLL.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using Microsoft.AspNet.Identity;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private IIdentityUnitOfWork Database { get; }

        public UserService(IIdentityUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Создает пользователя в системе
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user != null)
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
                
            user = new ApplicationUser
            {
                Email = userDto.Email,
                UserName = userDto.Email,
                Name = userDto.Name,
                IsEnabled = true
            };
            var result = await Database.UserManager.CreateAsync(user, userDto.Password);
            if (result.Errors.Any())
            {
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            }

            await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
            await Database.SaveAsync();

            return new OperationDetails(true, "Регистрация успешно пройдена", "");
        }

        /// <summary>
        /// Авторизирует пользователя в системе
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {
                claim = await Database.UserManager.CreateIdentityAsync(
                    user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            }
               
            return claim;
        }

        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="role">New user role</param>
        /// <param name="userId">User id</param>
        /// <returns>True when role changed</returns>
        public async Task<OperationDetails> ChangeUserRole(Role role, string userId)
        {
            var user = await Database.UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new OperationDetails(false, "Данный пользователь не найден", "");
            }

            var roles = new List<string>();
            foreach (var userRole in user.Roles)
            {
                var oldRole = await Database.RoleManager.FindByIdAsync(userRole.RoleId);
                roles.Add(oldRole.Name);
            }
            
            var result = await Database.UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());

            if (result.Succeeded)
            {
                result = await Database.UserManager.AddToRoleAsync(user.Id, role.ToString());
                await Database.SaveAsync();
            }

            if (result.Errors.Any())
            {
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            }

            return new OperationDetails(true, "Роль успешно изменена", "");
        }

        public async Task<bool?> IsUserEnabled(string email)
        {
            return await Database.UserManager.Users
                .Where(x => x.Email == email)
                .Select(x => x.IsEnabled)
                .FirstOrDefaultAsync();
        }

        public async Task ChangeIsEnabled(bool isEnabled, string id)
        {
            var user = await Database.UserManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.IsEnabled = isEnabled;
            await Database.SaveAsync();
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var usersDto = new List<UserDTO>();
            var users = await Database.UserManager.Users.Include(x => x.Claims).ToListAsync();

            foreach (var applicationUser in users)
            {
                var role = await Database.RoleManager.FindByIdAsync(applicationUser.Roles.FirstOrDefault()?.RoleId);
                var user = new UserDTO
                {
                    Id = applicationUser.Id,
                    Email = applicationUser.Email,
                    Name = applicationUser.Name,
                    Password = null,
                    Role = role.Name,
                    UserName = applicationUser.UserName,
                    IsEnabled = applicationUser.IsEnabled.HasValue && applicationUser.IsEnabled.Value
                };
                usersDto.Add(user);
            }

            return usersDto;
        }

        public async Task<OperationDetails> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var result = await Database.UserManager.ChangePasswordAsync(id, oldPassword, newPassword);

            if (result.Errors.Any())
            {
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            }

            return new OperationDetails(true, "Пароль успешно изменен", "");
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await Database.UserManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            await Database.UserManager.DeleteAsync(user);
            await Database.SaveAsync();

            return await Database.UserManager.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<UserDTO> GetUser(string email)
        {
            var user = await Database.UserManager.FindByEmailAsync(email);

            var role = await Database.RoleManager.FindByIdAsync(user.Roles.FirstOrDefault()?.RoleId);
            var userDTO = new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                Role = role.Name,
                UserName = user.UserName,
                IsEnabled = user.IsEnabled.HasValue && user.IsEnabled.Value
            };
            return userDTO;
        }

        public async Task UpdateUser(UserDTO model, bool isAdmin)
        {
            var user = await Database.UserManager.FindByIdAsync(model.Id);
            user.Name = model.Name;
            user.UserName = model.Email;
            if (isAdmin)
            {
                user.IsEnabled = model.IsEnabled;
            }
            
            await Database.SaveAsync();
            await Database.UserManager.SetEmailAsync(model.Id, model.Email);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
