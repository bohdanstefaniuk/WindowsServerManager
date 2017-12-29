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
        /// <param name="email">User email address</param>
        /// <returns>True when role changed</returns>
        public async Task<OperationDetails> ChangeUserRole(Role role, string email)
        {
            var user = await Database.UserManager.Users.Include(x => x.Claims).FirstOrDefaultAsync(x => x.Email == email);
            var roles = user.Claims.Where(x => x.ClaimType == ClaimTypes.Role).Select(x => x.ClaimValue);
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

        public async Task ChangeIsEnabled(bool isEnabled, string email)
        {
            var user = await Database.UserManager.Users.FirstOrDefaultAsync(x => x.Email == email);
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
                    UserName = applicationUser.UserName
                };
                usersDto.Add(user);
            }

            return usersDto;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
