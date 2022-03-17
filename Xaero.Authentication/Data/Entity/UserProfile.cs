using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xaero.Authentication.Data.Entity
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string OIdProvider { get; set; }
        public string OId { get; set; }
    }
}
