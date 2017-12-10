using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Infrastructure;
using BLL.Interfaces;
using DataAccessLayer.Entities;
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
                
            user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
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

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
