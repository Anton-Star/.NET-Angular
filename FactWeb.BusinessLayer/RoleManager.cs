using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class RoleManager : BaseManager<RoleManager, IRoleRepository, Role>
    {
        public RoleManager(IRoleRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets a role by name
        /// </summary>
        /// <param name="name">Name of the Role</param>
        /// <returns>Role entity object</returns>
        public Role Get(string name)
        {
            LogMessage("Get (RoleManager)");

            return base.Repository.Get(name);
        }

        /// <summary>
        /// Gets a role by name asynchronously
        /// </summary>
        /// <param name="name">Name of the Role</param>
        /// <returns>Role entity object</returns>
        public Task<Role> GetAsync(string name)
        {
            LogMessage("GetAsync (RoleManager)");

            return base.Repository.GetAsync(name);
        }

        /// <summary>
        /// Gets a role by name asynchronously
        /// </summary>
        /// <param name="name">Name of the Role</param>
        /// <returns>Role entity object</returns>
        public async Task<List<Role>> GetRolesByRoleAsync(int roleId)
        {
            LogMessage("GetRolesByRoleAsync (RoleManager)");

            var roleList = await base.Repository.GetAllAsync();

            if (roleId == (int)Constants.Role.PrimaryContact)
            {
                roleList = roleList.Where(x => x.Id == (int)Constants.Role.PrimaryContact
                                        || x.Id == (int)Constants.Role.QualityManager
                                        || x.Id == (int)Constants.Role.FACTAdministrator
                                        ).ToList();
            }
            else if (roleId == (int)Constants.Role.FACTAdministrator)
            {
                return roleList.Where(x => x.Id != (int)Constants.Role.Inspector).ToList();
            }
            else
            {
                roleList = roleList.Where(x => x.Id != (int)Constants.Role.PrimaryContact
                                               && x.Id != (int)Constants.Role.QualityManager
                                               && x.Id != (int)Constants.Role.FACTAdministrator
                                               && x.Id != (int)Constants.Role.FACTCoordinator
                                               && x.Id != (int)Constants.Role.FACTConsultantCoordinator
                                               && x.Id != (int)Constants.Role.Inspector
                                               ).ToList();
            }

            //Hide Non-System User per bug 2617
            return roleList.Where(x=>x.Id != (int)Constants.Role.NonSystemUser).ToList();
        }
    }
}
