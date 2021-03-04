using Microsoft.EntityFrameworkCore;
using MyIdp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Services
{
    public class SmsService : ISmsService
    {
        private readonly UserDbContext userDbContext;

        public SmsService(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        public async Task<bool> ValidatePhoneNumAsync(string phone, string code)
        {
            var user = await userDbContext.SysUser.FirstOrDefaultAsync(c => c.phone == phone);
            if(user == null)
            {
                return false;
            }
            if (code == "123456")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
