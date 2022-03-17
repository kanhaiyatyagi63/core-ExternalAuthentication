using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xaero.Authentication.Data.Entity;
using Xaero.Authentication.Enumerations;
using Xaero.Authentication.Managers.Abstracts;
using Xaero.Authentication.Repositories.Abstracts;

namespace Xaero.Authentication.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IUserManager _manager;

        public UserController(IUserRepository repository, IUserManager manager)
        {
            _repository = repository;
            _manager = manager;
        }

        [HttpGet, Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        [HttpGet, Route("/Profile")]
        public IActionResult Profile()
        {
            var claims = this.User.Claims.ToDictionary(x => x.Type, x => x.Value);
            return View(claims);
        }

        [HttpGet, Route("[controller]/Logout")]
        public IActionResult Logout()
        {
            _manager.LogOut(this.HttpContext);
            return LocalRedirect("~/");
        }

        [HttpGet, Route("[controller]/ExternalLogin")]
        public IActionResult ExternalLogin(string returnUrl, string provider = "google")
        {
            string authenticationScheme = string.Empty;

            switch (provider)
            {
                case OidcProviderType.Facebook:
                    authenticationScheme = FacebookDefaults.AuthenticationScheme;
                    break;
                default:
                    authenticationScheme = GoogleDefaults.AuthenticationScheme;
                    break;
            }

            var auth = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(LoginCallback), new { provider, returnUrl })
            };

            return new ChallengeResult(authenticationScheme, auth);
        }

        public async Task<IActionResult> LoginCallback(string provider, string returnUrl = "~/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(SocialAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest();

            var id = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            //var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name);

            var obj = new UserProfile
            {
                EmailAddress = email != null ? email.Value : "",
                OIdProvider = provider,
                OId = id != null ? id.Value : ""
            };

            await _repository.CreateExternalUserAsync(obj, HttpContext);

            return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl);
        }
    }
}
