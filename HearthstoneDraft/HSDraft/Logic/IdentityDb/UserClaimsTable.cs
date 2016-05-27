using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace HSDraft.Logic.IdentityDb
{
    /// <summary>
    /// Class that represents the UserClaims table in the MySQL Database
    /// </summary>
    public class UserClaimsTable
    {
        private ClaimsFile claimsFile;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTable()
        {
            claimsFile = FileDatabase.GetClaimsDb();
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            var userclaims = claimsFile.Claims.Where(x => x.UserId == userId);

            foreach (var item in userclaims)
            {
                Claim claim = new Claim(item.Type, item.Value);
                claims.AddClaim(claim);
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            var userclaims = claimsFile.Claims.Where(x => x.UserId == userId);
            foreach (var item in userclaims)
            {
                claimsFile.Claims.Remove(item);
            }

            claimsFile = FileDatabase.UpdateClaimsDb(claimsFile);
            return 1;
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, string userId)
        {
            var newclaim = new HsClaim();
            newclaim.UserId = userId;
            newclaim.Type = userClaim.Type;
            newclaim.Value = userClaim.Value;
            claimsFile.Claims.Add(newclaim);

            claimsFile = FileDatabase.UpdateClaimsDb(claimsFile);
            return 1;
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, Claim claim)
        {
            var userclaims = claimsFile.Claims.FirstOrDefault(x => x.UserId == user.Id && x.Type == claim.Type && x.Value == claim.Value);
            if (userclaims != null)
            {
                claimsFile.Claims.Remove(userclaims);
            }

            claimsFile = FileDatabase.UpdateClaimsDb(claimsFile);
            return 1;
        }
    }
}