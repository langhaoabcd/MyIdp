using MyIdp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Data
{
    public class UserDbContextSeedData
    {
        public static void SeedData(UserDbContext userDbContext)
        {
            if (!userDbContext.SysUser.Any())
            {
                var users = new List<SysUser>
                {
                    new SysUser
                    {
                        id="818727",
                        username="alice",
                        nickname="alice",
                        password="alice",
                        email="alice@qq.com",
                        phone="15966662345",
                        register_time = DateTime.Now
                    },
                   new SysUser
                    {
                        id="88421113",
                        username="bob",
                        nickname="bob",
                        password="bob",
                        email="bob@sina.com",
                        phone="17172121212",
                        register_time = DateTime.Now
                    },
                };
                userDbContext.SysUser.AddRange(users);
                userDbContext.SaveChanges();
            }
        }
    }
}
