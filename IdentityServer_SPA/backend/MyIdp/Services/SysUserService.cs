using Microsoft.EntityFrameworkCore;
using MyIdp.Data;
using MyIdp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Services
{
    public class SysUserService : ISysUserService
    {
        private readonly UserDbContext userDbContext;

        public SysUserService(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }
        public async Task<bool> ValidateUserAsync(string username, string passward)
        {
            var user = await userDbContext.SysUser.FirstOrDefaultAsync(c => c.username == username);
            if (user == null)
            {
                return false;
            }
            if (user.password == passward)
            {
                return true;
            } else
            {
                return false;
            }

        }
        public async Task<SysUser> FindByUsernameAsync(string username)
        {
            var user= await userDbContext.SysUser.FirstOrDefaultAsync(c => c.username == username);
            return user;
        }

        public async Task<SysUser> FindByIdAsync(string id)
        {
            var user = await userDbContext.SysUser.FirstOrDefaultAsync(c => c.id == id);
            return user;
        }

        public async Task<SysUser> FindByPhoneAsync(string phone)
        {
            var user = await userDbContext.SysUser.FirstOrDefaultAsync(c => c.phone == phone);
            return user;
        }
    }
}
