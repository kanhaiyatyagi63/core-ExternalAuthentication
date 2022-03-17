using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Xaero.Authentication.Data;
using Xaero.Authentication.Data.Entity;
using Xaero.Authentication.Repositories.Abstracts;

namespace Xaero.Authentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateExternalUserAsync(UserProfile up, HttpContext httpContext)
        {
            if (up != null)
            {
                UserProfile user = _context.UserProfiles
                                          .FirstOrDefault(x => x.OId == up.OId && 
                                                          x.OIdProvider == up.OIdProvider);

                if (user == null)
                {
                    user = up;

                    await _context.UserProfiles.AddAsync(user);

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            return false;
        }
    }
}
