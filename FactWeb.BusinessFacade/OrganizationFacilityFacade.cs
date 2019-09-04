using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FactWeb.Infrastructure.Constants;

namespace FactWeb.BusinessFacade
{
    public class OrganizationFacilityFacade
    {
        private readonly Container container;

        public OrganizationFacilityFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<OrganizationFacility> GetAll()
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();

            return organizationFacilityManager.GetAll();
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<OrganizationFacility> GetAllAsync()
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();

            return organizationFacilityManager.GetAllOrganizationFacility();
        }

        /// <summary>
        /// Add organiztion facility relation
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public bool AddRelation(int organizationId, int facilityId, bool relation, string currentUser)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            return organizationFacilityManager.AddRelation(organizationId, facilityId, relation, currentUser);
        }

        /// <summary>
        /// Add organiztion facility relation asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<bool> SaveRelationAsync(int organizationFacilityId, int organizationId, int facilityId, bool relation, string currentUser)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            var facilityManager = this.container.GetInstance<FacilityManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            var facility = facilityManager.GetById(facilityId);
            var org = organizationManager.GetById(organizationId);

            if (facility != null)
            {
                if (!facility.PrimaryOrganizationId.HasValue)
                {
                    facility.PrimaryOrganizationId = organizationId;
                    facility.UpdatedBy = currentUser;
                    facility.UpdatedDate = DateTime.Now;

                    facilityManager.Save(facility);
                }

                if (facility.FacilityDirectorId.HasValue)
                {

                    var groups = trueVaultManager.GetAllGroups();

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

                    trueVaultManager.AddUserToGroups(userOrgs, facility.FacilityDirector.DocumentLibraryUserId, groups, org.DocumentLibraryGroupId, org.Name);
                }
            }

            return organizationFacilityManager.SaveRelationAsync(organizationFacilityId, organizationId, facilityId, relation, currentUser);
        }

        /// <summary>
        /// Check if duplicate ralation exist
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public bool IsDuplicateRelation(int organizationFacilityId,int organizationId, int facilityId)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            return organizationFacilityManager.IsDuplicateRelation(organizationFacilityId, organizationId, facilityId);
        }

        /// <summary>
        /// Check if duplicate ralation exist asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<bool> IsDuplicateRelationAsync(int organizationFacilityId,int organizationId, int facilityId)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            return organizationFacilityManager.IsDuplicateRelationAsync(organizationFacilityId, organizationId, facilityId);
        }

        /// <summary>
        /// Delete a organization facility record frim database against Id
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        public bool DeleteRelation(int organizationFacilityId)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            return organizationFacilityManager.DeleteRelation(organizationFacilityId);
        }

        //public Task<List<OrganizationFacility>> GetSitesByOrganization(int? organizationId)
        //{
        //    var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();

        //    return organizationFacilityManager.SearchAsync(organizationId, facilityId);
        //}

        /// <summary>
        /// Delete a organization facility record frim database against Id asynchronously
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRelationAsync(int organizationFacilityId)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            return organizationFacilityManager.DeleteRelationAsync(organizationFacilityId);
        }

        /// <summary>
        /// Check busines rules and return appropriate message
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public string CheckBusinessRules(int organizationFacilityId, int organizationId, int facilityId, string currentUser)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            var facilityManager = this.container.GetInstance<FacilityManager>();

            string warningMessage = string.Empty;
            int serviceTypeId = 0;
            if (organizationFacilityId == 0)
            {
                serviceTypeId = facilityManager.GetById(facilityId).ServiceTypeId;

                if (serviceTypeId == (int) ServiceTypeEnum.ApheresisCollection || serviceTypeId == (int) ServiceTypeEnum.MarrowCollectionCT)
                {
                    var result =organizationFacilityManager.GetAll()
                            .Find(
                                x =>
                                    x.OrganizationId == organizationId &&
                                    x.Facility.ServiceTypeId == (int) ServiceTypeEnum.ProcessingCT);
                    if (result == null)
                        warningMessage = OrganizationFacilityConstant.OneProcessType;

                }
                else if (serviceTypeId == (int) ServiceTypeEnum.ClinicalProgramCT)
                {
                    var result =
                        organizationFacilityManager.GetAll()
                            .Find(
                                x =>
                                    (x.OrganizationId == organizationId &&
                                     x.Facility.ServiceTypeId == (int) ServiceTypeEnum.ProcessingCT) &&
                                    ((x.OrganizationId == organizationId &&
                                      x.Facility.ServiceTypeId == (int) ServiceTypeEnum.ApheresisCollection) ||
                                     (x.OrganizationId == organizationId &&
                                      x.Facility.ServiceTypeId == (int) ServiceTypeEnum.MarrowCollectionCT)));
                    if (result == null)
                        warningMessage = OrganizationFacilityConstant.OneProcessingOneApheresis;

                }
            }

            return warningMessage;
        }

        /// <summary>
        /// Check busines rules and return appropriate message asynchronously
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public Task<string> CheckBusinessRulesAsync(int organizationFacilityId, int organizationId, int facilityId, string currentUser)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            var facilityManager = this.container.GetInstance<FacilityManager>();

            string warningMessage = string.Empty;
            int serviceTypeId = 0;
            if (organizationFacilityId == 0)
            {
                serviceTypeId = facilityManager.GetById(facilityId).ServiceTypeId;

                if (serviceTypeId == (int) ServiceTypeEnum.ApheresisCollection ||
                    serviceTypeId == (int) ServiceTypeEnum.MarrowCollectionCT)
                {
                    var result =
                        organizationFacilityManager.GetAll()
                            .Find(
                                x =>
                                    x.OrganizationId == organizationId &&
                                    x.Facility.ServiceTypeId == (int) ServiceTypeEnum.ProcessingCT);
                    if (result == null)
                        warningMessage = OrganizationFacilityConstant.OneProcessType;

                }
                else if (serviceTypeId == (int) ServiceTypeEnum.ClinicalProgramCT)
                {
                    var result =
                        organizationFacilityManager.GetAll()
                            .Find(
                                x =>
                                    (x.OrganizationId == organizationId &&
                                     x.Facility.ServiceTypeId == (int) ServiceTypeEnum.ProcessingCT) &&
                                    ((x.OrganizationId == organizationId &&
                                      x.Facility.ServiceTypeId == (int) ServiceTypeEnum.ApheresisCollection) ||
                                     (x.OrganizationId == organizationId &&
                                      x.Facility.ServiceTypeId == (int) ServiceTypeEnum.MarrowCollectionCT)));
                    if (result == null)
                        warningMessage = OrganizationFacilityConstant.OneProcessingOneApheresis;

                }
            }

            return Task.FromResult(warningMessage);
        }

        /// <summary>
        /// Gets all organizationfacilities by organization id or facility id asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of OrganizationFacility objects</returns>
        public Task<List<OrganizationFacility>> SearchAsync(int? organizationId, int? facilityId)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();

            return organizationFacilityManager.SearchAsync(organizationId, facilityId);
        }        
       
        /// <summary>
        /// Gets all organizationfacilities by organization id or facility id
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of OrganizationFacility objects</returns>
        public List<OrganizationFacility> Search(int? organizationId, int? facilityId)
        {
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();

            return organizationFacilityManager.Search(organizationId, facilityId);
        }

        public List<OrganizationFacilityItems> GetOrgFacilities()
        {
            var orgFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();

            return orgFacilityManager.GetOrgFacilities();
        }
    }
}
