using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{

    public class OrganizationConsultantManager : BaseManager<OrganizationConsultantManager, IOrganizationConsultantRepository, OrganizationConsutant>
    {
        public OrganizationConsultantManager(IOrganizationConsultantRepository repository) : base(repository)
        {

        }

        /// <summary>
        /// Get all organization consultants form database
        /// </summary>
        /// <returns></returns>
        public List<OrganizationConsutant> GetOrganizationConsultants()
        {
            LogMessage("GetOrganizationConsultants (OrganizationConsultantManager)");

            return base.Repository.GetAll();
        }

        /// <summary>
        /// Get all organization consultants form database asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrganizationConsutant>> GetOrganizationConsultantsAsync()
        {
            LogMessage("GetOrganizationConsultantsAsync (OrganizationConsultantManager)");

            return await base.Repository.GetAllAsync();
        }

        /// <summary>
        /// Get all organization consultants form database by Organization Id asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrganizationConsutant>> GetOrganizationConsultantsByOrgIdAsync(int orgId)
        {
            LogMessage("GetOrganizationConsultantsByOrgIdAsync (OrganizationConsultantManager)");

            return await base.Repository.GetByOrgIdAsync(orgId);
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
            LogMessage("Save (OrganizationConsultantManager)");
            OrganizationConsutant organizationConsutant = null;
            if (organizationConsultantId == 0)
            {
                organizationConsutant = new OrganizationConsutant();
                organizationConsutant.OrganizationId = organizationId;
                organizationConsutant.ConsultantId = new Guid(consultantId);
                organizationConsutant.StartDate = string.IsNullOrEmpty(startDate) ? null : (DateTime?)Convert.ToDateTime(startDate);
                organizationConsutant.EndDate = string.IsNullOrEmpty(endDate) ? null : (DateTime?)Convert.ToDateTime(endDate);
                organizationConsutant.CreatedBy = currentUser;
                organizationConsutant.CreatedDate = DateTime.Now;

                base.Repository.AddAsync(organizationConsutant);
            }
            else
            {
                organizationConsutant = base.Repository.GetById(organizationConsultantId);
                organizationConsutant.OrganizationId = organizationId;
                organizationConsutant.ConsultantId = new Guid(consultantId);
                organizationConsutant.StartDate = string.IsNullOrEmpty(startDate) ? null : (DateTime?)Convert.ToDateTime(startDate);
                organizationConsutant.EndDate = string.IsNullOrEmpty(endDate) ? null : (DateTime?)Convert.ToDateTime(endDate);
                organizationConsutant.UpdatedBy = currentUser;
                organizationConsutant.UpdatedDate = DateTime.Now;

                base.Repository.SaveAsync(organizationConsutant);
            }

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
            LogMessage("SaveAsync (OrganizationConsultantManager)");
            OrganizationConsutant organizationConsutant = null;
            if (organizationConsultantId == 0)
            {
                organizationConsutant = new OrganizationConsutant();
                organizationConsutant.OrganizationId = organizationId;
                organizationConsutant.ConsultantId = new Guid(consultantId);
                organizationConsutant.StartDate = string.IsNullOrEmpty(startDate) ? null : (DateTime?)Convert.ToDateTime(startDate);
                organizationConsutant.EndDate = string.IsNullOrEmpty(endDate) ? null : (DateTime?)Convert.ToDateTime(endDate);
                organizationConsutant.CreatedBy = currentUser;
                organizationConsutant.CreatedDate = DateTime.Now;
                
                base.Repository.AddAsync(organizationConsutant);                                
            }
            else
            {
                organizationConsutant = base.Repository.GetById(organizationConsultantId);
                organizationConsutant.OrganizationId = organizationId;
                organizationConsutant.ConsultantId = new Guid(consultantId);
                organizationConsutant.StartDate = string.IsNullOrEmpty(startDate) ? null : (DateTime?)Convert.ToDateTime(startDate);
                organizationConsutant.EndDate = string.IsNullOrEmpty(endDate) ? null : (DateTime?)Convert.ToDateTime(endDate);
                organizationConsutant.UpdatedBy = currentUser;
                organizationConsutant.UpdatedDate = DateTime.Now;

                base.Repository.SaveAsync(organizationConsutant);
            }

            return Task.FromResult(true);

        }

        /// <summary>
        /// Deletes Organization Consultant record 
        /// </summary>
        /// <param name="organizationConsultantId"></param>
        /// <returns></returns>
        public bool Delete(int organizationConsultantId)
        {
            LogMessage("Delete (OrganizationConsultantManager)");

            OrganizationConsutant organizationConsutant = base.Repository.GetById(organizationConsultantId);
            base.Repository.Remove(organizationConsutant);

            return true;
        }

        /// <summary>
        /// Deletes Organization Consultant record asynchronously
        /// </summary>
        /// <param name="organizationConsultantId"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(int organizationConsultantId)
        {
            LogMessage("DeleteAsync (OrganizationConsultantManager)");

            OrganizationConsutant organizationConsutant = base.Repository.GetById(organizationConsultantId);
            base.Repository.RemoveAsync(organizationConsutant);

            return Task.FromResult(true);
        }

        /// <summary>
        /// Check if user enters duplicate record
        /// </summary>
        /// <param name="organizationConsultantItem"></param>
        /// <returns></returns>
        public bool IsDuplicateConsultant(int organizationConsultantId, int organizationId, string consultantId)
        {
            LogMessage("IsDuplicateConsultantAsync (OrganizationConsultantManager)");

            return base.Repository.IsDuplicateConsultant(organizationConsultantId, organizationId, consultantId);
        }

        /// <summary>
        /// Check if user enters duplicate record asynchronously
        /// </summary>
        /// <param name="organizationConsultantItem"></param>
        /// <returns></returns>
        public Task<bool> IsDuplicateConsultantAsync(int organizationConsultantId, int organizationId, string consultantId)
        {
            LogMessage("IsDuplicateConsultantAsync (OrganizationConsultantManager)");

            return base.Repository.IsDuplicateConsultantAsync(organizationConsultantId, organizationId, consultantId);
        }

    }
}


