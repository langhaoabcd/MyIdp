using Microsoft.EntityFrameworkCore;
using MyIdp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Data
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
           : base(options)
        {
        }

        public DbSet<SysUser> SysUser { get; set; }
    }
}
