using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;

namespace HSDraft.Logic.IdentityDb
{
    /// <summary>
    /// Class that represents the UserLogins table in the MySQL Database
    /// </summary>
    public class UserLoginsTable
    {
        private UserLoginFile loginFile;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsTable()
        {
            loginFile = FileDatabase.GetUserLoginDb();
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            var userlogin = loginFile.Logins.FirstOrDefault(x => x.UserId == user.Id && x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            loginFile.Logins.Remove(userlogin);
            loginFile = FileDatabase.UpdateLoginDb(loginFile);
            return 1;
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            var userlogin = loginFile.Logins.FirstOrDefault(x => x.UserId == userId);
            loginFile.Logins.Remove(userlogin);
            loginFile = FileDatabase.UpdateLoginDb(loginFile);
            return 1;
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            var newlogin = new HsUserLogin();
            newlogin.LoginProvider = login.LoginProvider;
            newlogin.ProviderKey = login.ProviderKey;
            newlogin.UserId = user.Id;

            loginFile.Logins.Add(newlogin);
            loginFile = FileDatabase.UpdateLoginDb(loginFile);
            return 1;
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            var login = loginFile.Logins.FirstOrDefault(x => x.ProviderKey == userLogin.ProviderKey && x.LoginProvider == userLogin.LoginProvider);
            if (login != null) return login.UserId;
            return null;
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            var userlogins = loginFile.Logins.Where(x => x.UserId == userId);

            foreach (var item in userlogins)
            {
                var login = new UserLoginInfo(item.LoginProvider, item.ProviderKey);
                logins.Add(login);
            }

            return logins;
        }
    }
}