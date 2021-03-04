using IdentityServer4.Models;
using IdentityServer4.Validation;
using MyIdp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace MyIdp.Validator
{
    /// <summary>
    /// 密码登录验证器
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ISysUserService sysUserService;

        public ResourceOwnerPasswordValidator(ISysUserService sysUserService)
        {
            this.sysUserService = sysUserService;
        }
        /// <summary>
        /// 资源所有者密码凭据类型，登录时验证账号密码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var validateUser = await sysUserService.ValidateUserAsync(context.UserName,context.Password);
            if(!validateUser)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,"账号或密码错误");
                return;
            }
            var user = await sysUserService.FindByUsernameAsync(context.UserName);
            context.Result = new GrantValidationResult
            (
               subject: user.id.ToString(),
               authenticationMethod: AuthenticationMethods.Password
            //这里返回clamins，是否profile就不用返回了？但是profile获取clamins各授权模式可以共用
            );
        }
    }
}
