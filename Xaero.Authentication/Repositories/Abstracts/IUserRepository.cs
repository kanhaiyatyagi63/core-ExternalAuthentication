using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xaero.Authentication.Data.Entity;

namespace Xaero.Authentication.Repositories.Abstracts
{
    public interface IUserRepository
    {
        Task<bool> CreateExternalUserAsync(UserProfile id, HttpContext httpContext);
    }
}
