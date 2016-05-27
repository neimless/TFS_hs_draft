using System.Collections.Generic;

namespace HSDraft.Logic.IdentityDb
{
    public class UserFile
    {
        public UserFile()
        {
            this.Users = new List<HsUser>();
        }

        public List<HsUser> Users { get; set; }
    }
}