using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class RoleFacade
    {
        private readonly Container container;

        public RoleFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get all the role 
        /// </summary>
        /// <returns></returns>
        public List<Role> GetAll()
        {
            var roleManager = this.container.GetInstance<RoleManager>();

            return roleManager.GetAll();
        }

        /// <summary>
        /// Get all the role asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<Role>> GetAllAsync()
        {
            var roleManager = this.container.GetInstance<RoleManager>();
            return roleManager.GetAllAsync();
        }
        
        /// <summary>
        /// Get all the role by role id asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<Role>> GetRolesByRoleAsync(int roleId)
        {
            var roleManager = this.container.GetInstance<RoleManager>();
            return roleManager.GetRolesByRoleAsync(roleId);
        }

        /// <summary>
        /// Gets a role by Id
        /// </summary>
        /// <param name="id">Id of the Role</param>
        /// <returns></returns>
        public Role GetById(Guid id)
        {
            var roleManager = this.container.GetInstance<RoleManager>();

            return roleManager.GetById(id);
        }
    }
}

