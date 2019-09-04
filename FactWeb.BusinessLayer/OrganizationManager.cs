using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class OrganizationManager : BaseManager<OrganizationManager, IOrganizationRepository, Organization>
    {
        public OrganizationManager(IOrganizationRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="organizationName">Name of the organization</param>
        /// <param name="city">City the organization is located in</param>
        /// <param name="state">State the organization is located in</param>
        /// <returns>Collection of organization objects</returns>
        public Task<List<Organization>> SearchAsync(string organizationName, string city, string state)
        {
            LogMessage("SearchAsync (OrganizationManager)");

            return base.Repository.SearchAsync(organizationName, city, state);
        }

        public List<Organization> GetAllOrganizations(bool includeFacilities)
        {
            LogMessage("SearchAsync (GetAllOrganizations)");

            return base.Repository.GetAllOrganizations(includeFacilities);
        }

        public List<Organization> GetAllOrganizationWithApplications()
        {
            return base.Repository.GetAllOrganizationWithApplications();
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="organizationName">Name of the organization</param>
        /// <param name="city">City the organization is located in</param>
        /// <param name="state">State the organization is located in</param>
        /// <returns>Collection of organization objects</returns>
        public List<Organization> Search(string organizationName, string city, string state)
        {
            LogMessage("Search (OrganizationManager)");

            return base.Repository.Search(organizationName, city, state);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of organization objects</returns>
        public Task<List<Organization>> SearchAsync(int? organizationId, int? facilityId)
        {
            LogMessage("SearchAsync (OrganizationManager)");

            if (organizationId.HasValue)
            {
                return Task.FromResult(new List<Organization> {base.Repository.GetById(organizationId.Value)});
            }
            else if (facilityId.HasValue)
            {
                return base.Repository.GetByFacilityAsync(facilityId.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all the organizations by facility id asynchronously
        /// </summary>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of organization objects</returns>
        public Task<List<Organization>> GetByFacilityIdAndRelation(int facilityId, bool strongRelation)
        {
            LogMessage("GetByFacilityIdAndRelation (OrganizationManager)");

            return base.Repository.GetByFacilityIdAndRelationAsync(facilityId, strongRelation);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of organization objects</returns>
        public List<Organization> Search(int? organizationId, int? facilityId)
        {
            LogMessage("Search (OrganizationManager)");

            if (organizationId.HasValue)
            {
                return new List<Organization> { base.Repository.GetById(organizationId.Value) };
            }
            else if (facilityId.HasValue)
            {
                return base.Repository.GetByFacility(facilityId.Value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an organization by its name
        /// </summary>
        /// <param name="organizationName">Name of the Organization</param>
        /// <returns>Organization entity object</returns>
        public Organization GetByName(string organizationName)
        {
            LogMessage("GetByName (OrganizationManager)");

            return base.Repository.GetByName(organizationName);
        }

        /// <summary>
        /// Gets an organization by its name asynchronously
        /// </summary>
        /// <param name="organizationName">Name of the Organization</param>
        /// <returns>Organization entity object</returns>
        public Task<Organization> GetByNameAsync(string organizationName)
        {
            LogMessage("GetByNameAsync (OrganizationManager)");

            return base.Repository.GetByNameAsync(organizationName);
        }

        /// <summary>
        /// Gets accredited services of an organization by its id asynchronously
        /// </summary>
        /// <param name="organizationID">Id of the Organization</param>
        public string GetAccreditedServices(int organizationId)
        {
            LogMessage("GetAccreditedServices (OrganizationManager)");

            return base.Repository.GetAccreditedServices(organizationId);
        }

        public Organization Save(OrganizationItem organizationItem, string email)
        {
            LogMessage("Save (OrganizationManager)");

            var organization = new Organization
            {
                Name = organizationItem.OrganizationName,
                Phone = organizationItem.OrganizationPhoneUS,
                PhoneExt = organizationItem.OrganizationPhoneUSExt,
                Fax = organizationItem.OrganizationFax,
                FaxExt = organizationItem.OrganizationFaxExt,
                Email = organizationItem.OrganizationEmail,
                WebSite = organizationItem.OrganizationWebSite,
                BAADocumentVersion = organizationItem.BAADocumentVersion,
                Comments = organizationItem.Comments,
                Description = organizationItem.Description,
                SpatialRelationship = organizationItem.SpatialRelationship,
                DocumentLibraryGroupId = organizationItem.DocumentLibraryGroupId,
                DocumentLibraryVaultId = organizationItem.DocumentLibraryVaultId,
                UseTwoYearCycle = organizationItem.UseTwoYearCycle,
                CcEmailAddresses = organizationItem.CcEmailAddresses,
                DocumentLibraries = new List<OrganizationDocumentLibrary>
                {
                    new OrganizationDocumentLibrary
                    {
                        Id = Guid.NewGuid(),
                        CycleNumber = organizationItem.CycleNumber.HasValue ? organizationItem.CycleNumber.GetValueOrDefault() : 1,
                        IsCurrent = true,
                        CreatedBy = email,
                        CreatedDate = DateTime.Now,
                        VaultId = organizationItem.DocumentLibraryVaultId
                    }
                },
                Users = new List<UserOrganization>()
            };

            //if (organizationItem.CycleNumber.HasValue && organizationItem.CycleEffectiveDate.HasValue)
            //{
            //    organization.OrganizationAccreditationCycles = new List<OrganizationAccreditationCycle>
            //    {
            //        new OrganizationAccreditationCycle
            //        {
            //            Id = Guid.NewGuid(),
            //            IsCurrent = true,
            //            Number = organizationItem.CycleNumber.Value,
            //            EffectiveDate = organizationItem.CycleEffectiveDate.Value,
            //            CreatedBy = email,
            //            CreatedDate = DateTime.Now
            //        }
            //    };
            //}

            if (organizationItem.OrganizationTypeItem != null)
            {
                organization.OrganizationTypeId = organizationItem.OrganizationTypeItem.OrganizationTypeId;
            }



            foreach (var director in organizationItem.OrganizationDirectors)
            {
                organization.Users.Add(new UserOrganization
                {
                    CreatedBy = email,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    JobFunctionId = Constants.JobFunctionIds.OrganizationDirector,
                    UserId = director.UserId.Value,
                    ShowOnAccReport = true
                });
            }

            if (organizationItem.PrimaryUser != null)
            {
                organization.PrimaryUserId = organizationItem.PrimaryUser.UserId;
            }

            if (organizationItem.AccreditationStatusItem != null)
            {
                organization.AccreditationStatusId = organizationItem.AccreditationStatusItem.Id;
            }

            if (organizationItem.BAAOwnerItem != null)
            {
                organization.BAAOwnerId = organizationItem.BAAOwnerItem.Id;
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.BAAExecutionDate))
            {
                organization.BAAExecutionDate = Convert.ToDateTime(organizationItem.BAAExecutionDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.BAAVerifiedDate))
            {
                organization.BAAVerifiedDate = Convert.ToDateTime(organizationItem.BAAVerifiedDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationDate))
            {
                organization.AccreditationDate = Convert.ToDateTime(organizationItem.AccreditationDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationExpirationDate))
            {
                organization.AccreditationExpirationDate = Convert.ToDateTime(organizationItem.AccreditationExpirationDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationExtensionDate))
            {
                organization.AccreditationExtensionDate = Convert.ToDateTime(organizationItem.AccreditationExtensionDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditedSince))
            {
                organization.AccreditedSince = Convert.ToDateTime(organizationItem.AccreditedSince);
            }

            if (organizationItem.UseTwoYearCycle && organization.AccreditationDate.HasValue)
            {
                organization.AccreditationExpirationDate = organization.AccreditationDate.Value.AddYears(2);
            }

            organization.CreatedBy = email;
            organization.CreatedDate = DateTime.Now;
            
            base.Repository.Add(organization);

            var org = this.GetById(organization.Id);

            org.Number = (org.Id + 399).ToString();

            base.Repository.Save(org);

            return organization;
            
        }

        public async Task<bool> UpdateOrganizationAsync(OrganizationItem organizationItem, string updatedBy)
        {
            var twoYearCycleChanged = false;

            LogMessage("UpdateOrganizationAsync (OrganizationManager)");

            var organization = base.Repository.GetById(organizationItem.OrganizationId);

            if (organization == null)
            {
                throw new ObjectNotFoundException("Cannot find organization.");
            }

            if (organization.UseTwoYearCycle.GetValueOrDefault() != organizationItem.UseTwoYearCycle)
            {
                twoYearCycleChanged = true;
            }

            organization.Name = organizationItem.OrganizationName;
            //organization.OrganizationDirectorId = organizationItem.OrganizationDirector.UserId;
            organization.PrimaryUserId = organizationItem.PrimaryUser.UserId;
            organization.AccreditationStatusId = organizationItem.AccreditationStatusItem.Id;
            organization.BAAOwnerId = organizationItem.BAAOwnerItem?.Id;
            organization.BaaDocumentPath = organizationItem.BaaDocumentPath;
            organization.UseTwoYearCycle = organizationItem.UseTwoYearCycle;
            organization.CcEmailAddresses = organizationItem.CcEmailAddresses;
            organization.Users = new List<UserOrganization>();

            foreach (var director in organizationItem.OrganizationDirectors)
            {
                organization.Users.Add(new UserOrganization
                {
                    CreatedBy = updatedBy,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    JobFunctionId = Constants.JobFunctionIds.OrganizationDirector,
                    UserId = director.UserId.Value,
                    ShowOnAccReport = true
                });
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.BAAExecutionDate))
            {
                organization.BAAExecutionDate = Convert.ToDateTime(organizationItem.BAAExecutionDate);
            }
            
            organization.BAADocumentVersion = organizationItem.BAADocumentVersion;

            if (!string.IsNullOrWhiteSpace(organizationItem.BAAVerifiedDate))
            { 
                organization.BAAVerifiedDate = Convert.ToDateTime(organizationItem.BAAVerifiedDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationDate))
            { 
                organization.AccreditationDate = Convert.ToDateTime(organizationItem.AccreditationDate);
            }

            if (string.IsNullOrWhiteSpace(organizationItem.AccreditationExtensionDate) && !string.IsNullOrWhiteSpace(organizationItem.AccreditationDate))
            {
                organization.AccreditationExpirationDate = Convert.ToDateTime(organizationItem.AccreditationDate).AddYears(3);
            }
            else if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationExtensionDate))
            {
                organization.AccreditationExpirationDate = Convert.ToDateTime(organizationItem.AccreditationExtensionDate);
            }

            if (twoYearCycleChanged && organizationItem.UseTwoYearCycle && organization.AccreditationDate.HasValue &&
                string.IsNullOrWhiteSpace(organizationItem.AccreditationExtensionDate))
            {
                organization.AccreditationExpirationDate = organization.AccreditationDate.Value.AddYears(2);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationExtensionDate))
            {
                organization.AccreditationExtensionDate = Convert.ToDateTime(organizationItem.AccreditationExtensionDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditedSince))
            {
                organization.AccreditedSince = Convert.ToDateTime(organizationItem.AccreditedSince);
            }
                        
            organization.Comments = organizationItem.Comments;
            organization.Description = organizationItem.Description;
            organization.SpatialRelationship = organizationItem.SpatialRelationship;

            var hasCycleNumber = false;
            foreach(var cycle in organization.OrganizationAccreditationCycles)
            {
                cycle.IsCurrent = false;
                if (organizationItem.CycleNumber == cycle.Number)
                {
                    cycle.IsCurrent = true;
                    hasCycleNumber = true;
                    if (organizationItem.CycleEffectiveDate.HasValue)
                        cycle.EffectiveDate = organizationItem.CycleEffectiveDate.Value;
                }
            }
            if (!hasCycleNumber && (organizationItem.CycleNumber > 0 || organizationItem.CycleEffectiveDate.HasValue))
            {
                OrganizationAccreditationCycle cycle = new OrganizationAccreditationCycle()
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = organization.Id,
                    Number = organizationItem.CycleNumber.Value,
                    EffectiveDate = organizationItem.CycleEffectiveDate.Value,
                    IsCurrent = true,
                    CreatedBy = updatedBy,
                    CreatedDate = DateTime.Now
                };
                organization.OrganizationAccreditationCycles.Add(cycle);
            }

            organization.UpdatedBy = updatedBy;
            organization.UpdatedDate = DateTime.Now;

            await this.Repository.SaveAsync(organization);

            return twoYearCycleChanged;
        }

        /// <summary>
        /// Gets all organizations that have submitted eligibility applications
        /// </summary>
        /// <returns>Collection of Organizations</returns>
        public List<Organization> GetAllWithSubmittedEligibility()
        {
            return base.Repository.GetAllWithSubmittedEligibility();
        }

        /// <summary>
        /// Gets all organizations that have submitted eligibility applications asynchronously
        /// </summary>
        /// <returns>Collection of Organizations</returns>
        public Task<List<Organization>> GetAllWithSubmittedEligibilityAsync()
        {
            return base.Repository.GetAllWithSubmittedEligibilityAsync();
        }

        /// <summary>
        /// Returns whether the user has access to the org data or not
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Boolean if the user has access or not</returns>
        public bool DoesHaveAccess(string orgName, Guid userId)
        {
            return base.Repository.DoesHaveAccess(orgName, userId);
        }

        public void ChangeOrgToAccredited(Organization organization, AccreditationStatus accreditedAccreditationStatus,
            string updatedBy)
        {

            if (organization.AccreditationStatus.Name == Constants.AccreditationStatuses.Accredited)
            {

                if (organization.AccreditationDate.HasValue)
                {
                    organization.AccreditationDate = organization.UseTwoYearCycle.GetValueOrDefault()
                        ? organization.AccreditationDate.Value.AddYears(2)
                        : organization.AccreditationDate.Value.AddYears(3);
                }
                else
                {
                    organization.AccreditationDate = DateTime.Now;
                }

                if (organization.AccreditationExpirationDate.HasValue)
                {
                    organization.AccreditationExpirationDate = organization.UseTwoYearCycle.GetValueOrDefault()
                    ? organization.AccreditationExpirationDate.Value.AddYears(2)
                    : organization.AccreditationExpirationDate.Value.AddYears(3);
                }
                else
                {
                    organization.AccreditationExpirationDate = organization.UseTwoYearCycle.GetValueOrDefault()
                        ? DateTime.Now.AddYears(2)
                        : DateTime.Now.AddYears(3);
                }                
            }
            else
            {
                organization.AccreditationDate = DateTime.Now;
                organization.AccreditationExpirationDate = organization.UseTwoYearCycle.GetValueOrDefault()
                    ? DateTime.Now.AddYears(2)
                    : DateTime.Now.AddYears(3);
            }

            organization.AccreditationStatusId = accreditedAccreditationStatus.Id;
            organization.AccreditationExtensionDate = null;
            organization.UpdatedDate = DateTime.Now;
            organization.UpdatedBy = updatedBy;

            base.Repository.Save(organization);
        }

        public void ChangeOrgToAccredited(int orgId, AccreditationStatus accreditedAccreditationStatus, string updatedBy)
        {
            var organization = this.GetById(orgId);

            if (organization == null) return;

            this.ChangeOrgToAccredited(organization, accreditedAccreditationStatus, updatedBy);

        }

        public List<Organization> GetOrganizationsWithoutDocumentLibrary()
        {
            return base.Repository.GetOrganizationsWithoutDocumentLibrary();
        }

        public Task<List<Organization>> GetOrganizationsWithoutDocumentLibraryAsync()
        {
            return base.Repository.GetOrganizationsWithoutDocumentLibraryAsync();
        }

        public bool HasQmRestriction(int organizationId)
        {
            return base.Repository.HasQmRestriction(organizationId);
        }

        public List<UserOrganizationItem> FillTrueVaultGroupsForUsers(List<UserOrganizationItem> items)
        {
            var results = new List<UserOrganizationItem>();

            foreach (var item in items)
            {
                if (item.Organization == null) continue;

                if (string.IsNullOrEmpty(item.Organization.DocumentLibraryGroupId))
                {
                    var org = this.GetByName(item.Organization.OrganizationName);

                    if (org != null)
                    {
                        item.Organization.DocumentLibraryGroupId = org.DocumentLibraryGroupId;
                    }
                }

                results.Add(item);
            }

            return results;
        }

        public void CreateDocumentLibrary(int organizationId, int cycleNumber, string vaultId, string groupId, string createdBy)
        {
            base.Repository.CreateDocumentLibrary(organizationId, cycleNumber, vaultId, groupId, createdBy);
        }

        public List<SimpleOrganization> GetSimpleOrganizations()
        {
            return base.Repository.GetSimpleOrganizations();
        }

        public Organization GetByCompAppId(Guid compAppId)
        {
            return base.Repository.GetByCompAppId(compAppId);
        }
    }
}
