using MyIdp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Services
{
    public interface ISysUserService
    {
        Task<bool> ValidateUserAsync(string username,string passward);
        Task<SysUser> FindByUsernameAsync(string username);
        Task<SysUser> FindByIdAsync(string id);

        Task<SysUser> FindByPhoneAsync(string phone);
    }
}
