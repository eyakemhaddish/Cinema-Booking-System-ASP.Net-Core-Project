using System.Collections.Generic;
using System.Security.Claims;

namespace FromScratch_.net_5.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create Movie", "Create Movie"),
             new Claim("Edit Movie", "Edit Movie"),
              new Claim("Delete Movie", "Delete Movie")
        };
    }
}
