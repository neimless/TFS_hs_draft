using HSDraft.Logic.IdentityDb;
using System.Collections.Generic;

namespace HSDraft.Models.Maintenance
{
    public class Maintenance
    {
        public History.History FinishedDrafts { get; set; }

        public UserList Users { get; set; }
    }
}