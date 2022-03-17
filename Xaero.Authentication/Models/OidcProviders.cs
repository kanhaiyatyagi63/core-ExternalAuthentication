using System.Collections.Generic;

namespace Xaero.Authentication.Models
{
    public class OidcProviders
    {
        public List<OidcProvider> Providers { get; set; }
    }

    public class OidcProvider
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
