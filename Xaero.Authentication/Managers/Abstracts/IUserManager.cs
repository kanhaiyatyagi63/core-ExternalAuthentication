using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xaero.Authentication.Data.Entity;

namespace Xaero.Authentication.Managers.Abstracts
{
    public interface IUserManager
    {
        Task LogIn(HttpContext httpContext, UserProfile user, bool isPersistent = false);
        Task LogOut(HttpContext httpContext);
    }
}
