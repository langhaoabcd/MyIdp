using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MyIdp.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyIdp.Filter;
using MyIdp.Models;
using MyIdp.Models.Ids4Model;
using MyIdp.Models.Ids4Model.Ids4;
using MyIdp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MyIdp.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly ISysUserService _sysUserService;
        private readonly ISmsService _smsService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            ISysUserService sysUserService,
            ISmsService smsService,
            ILogger<AccountController> logger
            )
        {
            _interaction = interaction;
            _events = events;
            this._sysUserService = sysUserService;
            this._smsService = smsService;
            _logger = logger;
        }

        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginInput model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            // the user clicked the "cancel" button
            if (model.Cancle)
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
            }

            var validateUser = await _sysUserService.ValidateUserAsync(model.Username, model.Password);
            // validate username/password against in-memory store
            //if (_users.ValidateCredentials(model.Username, model.Password))
            if (validateUser)
            {
                var user = await _sysUserService.FindByUsernameAsync(model.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.username, user.id, user.username, clientId: context?.Client.ClientId));

                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                if (model.RememberLogin)
                {
                    props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.Now.AddHours(2)
                };
                };

                // issue authentication cookie with subject ID and username
                var isuser = new IdentityServerUser(user.id)
                {
                    DisplayName = user.username
                };

                await HttpContext.SignInAsync(isuser, props);

                // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                // the IsLocalUrl check is only necessary if you want to support additional local pages, otherwise IsValidReturnUrl is more strict
                if (context != null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    //return Redirect(model.ReturnUrl);
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                    //return Redirect(model.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(model.ReturnUrl))
                {

                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
                else
                {
                    // user might have clicked on a malicious link - should be logged
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
            throw new Exception("操作失败！");
        }


        /// <summary>
        /// 短信登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginBySms(LoginSmsInput model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            // the user clicked the "cancel" button
            if (model.Cancle)
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
            }

            var validateUser = await _smsService.ValidatePhoneNumAsync(model.Phone, model.Code);
            if (validateUser)
            {
                var user = await _sysUserService.FindByPhoneAsync(model.Phone);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.username, user.id, user.username, clientId: context?.Client.ClientId));

                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                if (model.RememberLogin)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.Now.AddHours(2)
                    };
                };

                // issue authentication cookie with subject ID and username
                var isuser = new IdentityServerUser(user.id)
                {
                    DisplayName = user.username
                };

                await HttpContext.SignInAsync(isuser, props);

                // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                // the IsLocalUrl check is only necessary if you want to support additional local pages, otherwise IsValidReturnUrl is more strict
                if (context != null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    //return Redirect(model.ReturnUrl);
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                    //return Redirect(model.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(model.ReturnUrl))
                {

                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
                else
                {
                    // user might have clicked on a malicious link - should be logged
                    return new JsonResult(new ReponseModel { Data = model.ReturnUrl });
                }
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(model.Phone, "invalid credentials", clientId: context?.Client.ClientId));
            throw new Exception("操作失败！");
        }


        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout(LogoutInput input)
        {
            // build a model so the logged out page knows what to display
            //var vm = await BuildLoggedOutViewModelAsync(input.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
                await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            //if (vm.TriggerExternalSignout)
            //{
            //    // build a return URL so the upstream provider will redirect back
            //    // to us after the user has logged out. this allows us to then
            //    // complete our single sign-out processing.
            //    string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            //    // this triggers a redirect to the external provider for sign-out
            //    return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            //}
            return new JsonResult(new ReponseModel { Data = "" });
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetErrorMsg(string errorId)
        {
            var err = await _interaction.GetErrorContextAsync(errorId);
            return new JsonResult(new ReponseModel { Data = err.ErrorDescription });
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = true,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }
            return vm;
        }


        /// <summary>
        /// 使用三方登录
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExternalLogin(string scheme, string returnUrl)
        {
            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(ExternalLoginCallback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
            };
            ChallengeResult res = Challenge(props, scheme);
            return res;
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                _logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = FindUserFromExternalProvider(result);

            // this allows us to collect any additional claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            var isuser = new IdentityServerUser(user.id)
            {
                DisplayName = user.username,
                IdentityProvider = provider,
                AdditionalClaims = (ICollection<Claim>)claims
            };

            await HttpContext.SignInAsync(isuser, localSignInProps);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.id, user.username, true, context?.Client.ClientId));

            return Redirect(returnUrl);
        }

        private (SysUser user, string provider, string providerUserId, IEnumerable<Claim> claims) FindUserFromExternalProvider(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            //var user = _users.FindByExternalProvider(provider, providerUserId);
            var user = new SysUser
            {
                id = providerUserId,
                username = claims.First().Value,
            };

            return (user, provider, providerUserId, claims);
        }


        //// if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        //// this will be different for WS-Fed, SAML2p or other protocols
        private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }
        }
    }
}
