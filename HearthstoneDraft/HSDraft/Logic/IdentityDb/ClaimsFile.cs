using System.Collections.Generic;

namespace HSDraft.Logic.IdentityDb
{
    public class ClaimsFile
    {
        public ClaimsFile()
        {
            this.Claims = new List<HsClaim>();
        }

        public List<HsClaim> Claims { get; set; }
    }
}