using System.Collections.Generic;
namespace HSDraft.Logic.IdentityDb
{
    public class RoleFile
    {
        public RoleFile()
        {
            this.Roles = new List<HsRole>();
        }

        public List<HsRole> Roles { get; set; }
    }
}