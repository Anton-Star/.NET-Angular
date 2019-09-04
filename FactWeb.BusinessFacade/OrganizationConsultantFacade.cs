using FactWeb.BusinessLayer;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class OrganizationConsultantFacade
    {
        private readonly Container container;

        public OrganizationConsultantFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get all organization consultants form database
        /// </summary>
        /// <returns></returns>
        public List<OrganizationConsutant> GetOrganizationConsultants()
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();

            return organizationConsultantManager.GetOrganizationConsultants();
        }

        /// <summary>
        /// Get all organization consultants form database asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<OrganizationConsutant>> GetOrganizationConsultantsAsync()
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();

            return organizationConsultantManager.GetOrganizationConsultantsAsync();
        }

        /// <summary>
        /// Get all organization consultants form database by organization id asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<OrganizationConsutant>> GetOrganizationConsultantsByOrgIdAsync(int orgId)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();

            return organizationConsultantManager.GetOrganizationConsultantsByOrgIdAsync(orgId);
        }

        /// <summary>
        /// Saves Organization Consultant record 
        /// </summary>
        /// <param name="organizationConsultantId"></param>
        /// <param name="organizationId"></param>
        /// <param name="consultantId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool Save(int organizationConsultantId, int organizationId, string consultantId, string startDate, string endDate, string currentUser)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var result = organizationConsultantManager.Save(organizationConsultantId, organizationId, consultantId, startDate, endDate, currentUser);

            if (!result) return false;

            var organization = organizationManager.GetById(organizationId);
            var user = userManager.GetById(new Guid(consultantId));

            if (organization == null || user == null) return true;

            var groups = trueVaultManager.GetAllGroups();

            var userOrgs = new List<UserOrganizationItem>
            {
                new UserOrganizationItem
                {
                    Organization = new OrganizationItem
                    {
                        OrganizationName = organization.Name,
                        DocumentLibraryGroupId = organization.DocumentLibraryGroupId
                    }
                }
            };

            trueVaultManager.AddUserToGroups(userOrgs, user.DocumentLibraryUserId, groups);

            return true;
        }

        /// <summary>
        /// Saves Organization Consultant record asynchronously
        /// </summary>
        /// <param name="organizationConsultantId"></param>
        /// <param name="organizationId"></param>
        /// <param name="consultantId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<bool> SaveAsync(int organizationConsultantId, int organizationId, string consultantId, string startDate, string endDate, string currentUser)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var org = orgManager.GetById(organizationId);
            var user = userManager.GetById(new Guid(consultantId));

            var groups = trueVaultManager.GetAllGroups();

            if (groups.Result != TrueVaultManager.Success)
            {
                throw new Exception("Cannot get True Vault Groups");
            }

            var userOrgs = new List<UserOrganizationItem>
            {
                new UserOrganizationItem
                {
                    Organization = new OrganizationItem
                    {
                        OrganizationName = org.Name,
                        DocumentLibraryGroupId = org.DocumentLibraryGroupId
                    }
                }
            };

            trueVaultManager.AddUserToGroups(userOrgs, user.DocumentLibraryUserId, groups);


            return organizationConsultantManager.SaveAsync(organizationConsultantId, organizationId, consultantId, startDate, endDate, currentUser);
        }

        /// <summary>
        /// Deletes Organization Consultant record 
        /// </summary>
        /// <param name="organizationConsultantId"></param>
        /// <returns></returns>
        public bool Delete(int organizationConsultantId)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();
            return organizationConsultantManager.Delete(organizationConsultantId);
        }

        /// <summary>
        /// Deletes Organization Consultant record asynchronously
        /// </summary>
        /// <param name="organizationConsultantId"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(int organizationConsultantId)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();
            return organizationConsultantManager.DeleteAsync(organizationConsultantId);
        }

        /// <summary>
        /// Check if user enters duplicate record
        /// </summary>
        /// <param name="organizationConsultantItem"></param>
        /// <returns></returns>
        public bool IsDuplicateConsultant(OrganizationConsultantItem organizationConsultantItem)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();
            return organizationConsultantManager.IsDuplicateConsultant(organizationConsultantItem.OrganizationConsultantId, organizationConsultantItem.OrganizationId, organizationConsultantItem.ConsultantId);
        }

        /// <summary>
        /// Check if user enters duplicate record asynchronously
        /// </summary>
        /// <param name="organizationConsultantItem"></param>
        /// <returns></returns>
        public Task<bool> IsDuplicateConsultantAsync(OrganizationConsultantItem organizationConsultantItem)
        {
            var organizationConsultantManager = this.container.GetInstance<OrganizationConsultantManager>();
            return organizationConsultantManager.IsDuplicateConsultantAsync(organizationConsultantItem.OrganizationConsultantId, organizationConsultantItem.OrganizationId, organizationConsultantItem.ConsultantId);
        }

    }
}
