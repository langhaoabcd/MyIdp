using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MyIdp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyIdp.Validator
{
    public class UserProfileService : IProfileService
    {
        private readonly ISysUserService sysUserService;

        public UserProfileService(ISysUserService sysUserService)
        {
            this.sysUserService = sysUserService;
        }
        /// <summary>
        /// 获取用户clamins
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject?.GetSubjectId();
            if (subjectId == null)
            {
                throw new Exception("subjectId为空");
            }
            var user = await sysUserService.FindByIdAsync(subjectId);
            if (user == null)
            {
                return;
                //throw new Exception($"用户{subjectId}不存在");
            }
            var clamins = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.nickname),
                new Claim(JwtClaimTypes.Email, user.email),
                new Claim(JwtClaimTypes.PhoneNumber,user.phone),
                new Claim(JwtClaimTypes.WebSite, "http://huzhihao.cn"),
                new Claim(JwtClaimTypes.Address, "China")
            };
            context.IssuedClaims = clamins;
        }

        /// <summary>
        /// 判断用户是否有效
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            //验证用户是否有效
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("用户Id为空");
            return Task.CompletedTask;
        }
    }
}
