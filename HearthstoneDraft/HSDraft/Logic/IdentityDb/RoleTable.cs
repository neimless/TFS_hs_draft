using System;
using System.Collections.Generic;
using System.Linq;

namespace HSDraft.Logic.IdentityDb
{
    /// <summary>
    /// Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleTable
    {
        private RoleFile roleFile;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public RoleTable()
        {
            roleFile = FileDatabase.GetRoleDb();
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            var role = roleFile.Roles.FirstOrDefault(x => x.Id == roleId);
            roleFile.Roles.Remove(role);
            return 1;
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public int Insert(IdentityRole role)
        {
            var newrole = new HsRole();
            newrole.Id = role.Id;
            newrole.Name = role.Name;
            roleFile.Roles.Add(newrole);
            return 1;
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(string roleId)
        {
            var role = roleFile.Roles.FirstOrDefault(x => x.Id == roleId);
            if (role != null)
            {
                return role.Name;
            }
            return null;
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            var role = roleFile.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                return role.Id;
            }
            return null;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if (roleName != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;

        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;
        }

        public int Update(IdentityRole role)
        {
            var roletoModify = GetRoleById(role.Id);
            roletoModify.Name = role.Name;
            return 1;
        }
    }
}