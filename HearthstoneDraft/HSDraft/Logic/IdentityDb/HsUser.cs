using System;

namespace HSDraft.Logic.IdentityDb
{
    public class HsUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string RoleId { get; set; }
        public string SecurityStamp { get; set; }
        public string Phonenumber { get; set; }
        public bool PhonenumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}