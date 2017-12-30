using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Infrastructure;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task<OperationDetails> ChangeUserRole(Role role, string email);
        Task<List<UserDTO>> GetUsers();
        Task<bool?> IsUserEnabled(string email);
        Task ChangeIsEnabled(bool isEnabled, string id);
        Task<bool> DeleteUser(string id);
        Task<UserDTO> GetUser(string email);
        Task UpdateUser(UserDTO model, bool isAdmin);
    }
}