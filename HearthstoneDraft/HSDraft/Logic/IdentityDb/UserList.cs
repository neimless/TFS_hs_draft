using System.Collections.Generic;

namespace HSDraft.Logic.IdentityDb
{
    public class UserList
    {
        public UserList()
        {
            this.Users = new List<UserInfo>();
        }

        public List<UserInfo> Users { get; set; }
    }
}