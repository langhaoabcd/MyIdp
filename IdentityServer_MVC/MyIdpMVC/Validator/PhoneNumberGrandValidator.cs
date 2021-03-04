using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MyIdp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Validator
{
    /// <summary>
    /// 短信登录验证器
    /// </summary>
    public class PhoneNumberGrandValidator : IExtensionGrantValidator
    {
        private readonly ISmsService smsService;
        private readonly ISysUserService sysUserService;

        public PhoneNumberGrandValidator(ISmsService smsService, ISysUserService sysUserService)
        {
            this.smsService = smsService;
            this.sysUserService = sysUserService;
        }
        public string GrantType => "phone_number";


        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var raw = context.Request.Raw;
            var phone = raw.Get("phone");
            var code = raw.Get("code");
            var grantType = raw.Get(OidcConstants.TokenRequest.GrantType);
            if (grantType != GrantType)
            {
                await Task.CompletedTask;
            }
            else
            {
                var validateUser = await smsService.ValidatePhoneNumAsync(phone, code);
                if (!validateUser)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "手机号或验证码错误");
                    return;
                }
                var user = await sysUserService.FindByPhoneAsync(phone);
                context.Result = new GrantValidationResult
                (
                    subject: user.id, 
                    authenticationMethod: OidcConstants.AuthenticationMethods.ConfirmationBySms
                );
            }
        }
    }
}
