using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Infrastructure;
using DataAccessLayer.Enums;

namespace BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task<OperationDetails> ChangeUserRole(Role role, string email);
    }
}