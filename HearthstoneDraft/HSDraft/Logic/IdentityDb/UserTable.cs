using System;
using System.Collections.Generic;
using System.Linq;

namespace HSDraft.Logic.IdentityDb
{
    /// <summary>
    /// Class that represents the Users table
    /// </summary>
    public class UserTable<TUser>
        where TUser : IdentityUser
    {
        private UserFile userFile;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable()
        {
            userFile = FileDatabase.GetUserDb();
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            var user = userFile.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return user.UserName;
            }

            return null;
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            var user = userFile.Users.FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                return user.Id;
            }

            return null;
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
            var dbuser = userFile.Users.FirstOrDefault(x => x.Id == userId);
            if (dbuser != null)
            {
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = dbuser.Id;
                user.UserName = dbuser.UserName;
                user.PasswordHash = dbuser.PasswordHash;
                user.SecurityStamp = dbuser.SecurityStamp;
                user.EmailConfirmed = dbuser.EmailConfirmed;
                user.Email = dbuser.Email;
                user.PhoneNumber = dbuser.Phonenumber;
                user.PhoneNumberConfirmed = dbuser.PhonenumberConfirmed;
                user.LockoutEnabled = dbuser.LockoutEnabled;
                user.TwoFactorEnabled = dbuser.TwoFactorEnabled;
                user.LockoutEndDateUtc = dbuser.LockoutEndDateUtc;
                user.LockoutEnabled = dbuser.LockoutEnabled;
                user.AccessFailedCount = dbuser.AccessFailedCount;
            }

            return user;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public TUser GetUserByName(string userName)
        {
            TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
            var dbuser = userFile.Users.FirstOrDefault(x => x.UserName == userName);
            if (dbuser != null)
            {
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = dbuser.Id;
                user.UserName = dbuser.UserName;
                user.PasswordHash = dbuser.PasswordHash;
                user.SecurityStamp = dbuser.SecurityStamp;
                user.EmailConfirmed = dbuser.EmailConfirmed;
                user.Email = dbuser.Email;
                user.PhoneNumber = dbuser.Phonenumber;
                user.PhoneNumberConfirmed = dbuser.PhonenumberConfirmed;
                user.LockoutEnabled = dbuser.LockoutEnabled;
                user.TwoFactorEnabled = dbuser.TwoFactorEnabled;
                user.LockoutEndDateUtc = dbuser.LockoutEndDateUtc;
                user.LockoutEnabled = dbuser.LockoutEnabled;
                user.AccessFailedCount = dbuser.AccessFailedCount;
                return user;
            }

            return null;
        }

        public TUser GetUserByEmail(string email)
        {
            TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
            var dbuser = userFile.Users.FirstOrDefault(x => x.Email == email);
            if (dbuser != null)
            {
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = dbuser.Id;
                user.UserName = dbuser.UserName;
                user.PasswordHash = dbuser.PasswordHash;
                user.SecurityStamp = dbuser.SecurityStamp;
                user.EmailConfirmed = dbuser.EmailConfirmed;
                user.Email = dbuser.Email;
                user.PhoneNumber = dbuser.Phonenumber;
                user.PhoneNumberConfirmed = dbuser.PhonenumberConfirmed;
                user.LockoutEnabled = dbuser.LockoutEnabled;
                user.TwoFactorEnabled = dbuser.TwoFactorEnabled;
                user.LockoutEndDateUtc = dbuser.LockoutEndDateUtc;
                user.LockoutEnabled = dbuser.LockoutEnabled;
                user.AccessFailedCount = dbuser.AccessFailedCount;
                return user;
            }

            return null;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            var user = userFile.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return user.PasswordHash;
            }

            return null;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            var existinguser = userFile.Users.FirstOrDefault(x => x.Id == userId);
            if (existinguser != null)
            {
                existinguser.PasswordHash = passwordHash;
                userFile = FileDatabase.UpdateUserDb(userFile);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            var existinguser = userFile.Users.FirstOrDefault(x => x.Id == userId);
            if (existinguser != null)
            {
                return existinguser.SecurityStamp;
            }

            return null;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            var existinguser = userFile.Users.FirstOrDefault(x => x.Id == user.Id);
            if (existinguser != null)
            {
                return 0;
            }

            var dbuser = new HsUser();
            dbuser.Id = user.Id;
            dbuser.PasswordHash = user.PasswordHash;
            dbuser.UserName = user.UserName;
            dbuser.Email = user.Email;
            dbuser.SecurityStamp = user.SecurityStamp;
            dbuser.EmailConfirmed = user.EmailConfirmed;
            dbuser.Phonenumber = user.PhoneNumber;
            dbuser.PhonenumberConfirmed = user.PhoneNumberConfirmed;
            dbuser.AccessFailedCount = user.AccessFailedCount;

            dbuser.LockoutEnabled = user.LockoutEnabled;
            dbuser.LockoutEndDateUtc = user.LockoutEndDateUtc;
            dbuser.TwoFactorEnabled = user.TwoFactorEnabled;

            userFile.Users.Add(dbuser);
            userFile = FileDatabase.UpdateUserDb(userFile);
            return 1;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            var user = userFile.Users.FirstOrDefault(x => x.Id == userId);
            userFile.Users.Remove(user);
            return 1;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        public int AddRoleToUser(string roleId, TUser user)
        {
            var dbuser = userFile.Users.FirstOrDefault(x => x.Id == user.Id);
            if (dbuser == null)
            {
                return 0;
            }

            dbuser.RoleId = roleId;
            return 1;
        }

        public string GetUserRoleId(TUser user)
        {
            var dbuser = userFile.Users.FirstOrDefault(x => x.Id == user.Id);
            if (dbuser != null)
            {
                return dbuser.RoleId;
            }

            return string.Empty;
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            return 1;
//            string commandText = @"Update Users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
//                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
//                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  
//                WHERE Id = @userId";
//            Dictionary<string, object> parameters = new Dictionary<string, object>();
//            parameters.Add("@userName", user.UserName);
//            parameters.Add("@pswHash", user.PasswordHash);
//            parameters.Add("@secStamp", user.SecurityStamp);
//            parameters.Add("@userId", user.Id);
//            parameters.Add("@email", user.Email);
//            parameters.Add("@emailconfirmed", user.EmailConfirmed);
//            parameters.Add("@phonenumber", user.PhoneNumber);
//            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
//            parameters.Add("@accesscount", user.AccessFailedCount);
//            parameters.Add("@lockoutenabled", user.LockoutEnabled);
//            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
//            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

//            return userFile.Execute(commandText, parameters);
        }
    }
}