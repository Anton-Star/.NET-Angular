using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class OrganizationFacade
    {
        private readonly Container container;

        public OrganizationFacade(Container container)
        {
            this.container = container;
        }

        public List<Organization> GetAllOrganizationWithApplications()
        {
            var manager = this.container.GetInstance<OrganizationManager>();

            return manager.GetAllOrganizationWithApplications();
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public List<Organization> GetAll(bool includeFacilities)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.GetAllOrganizations(includeFacilities);
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public Task<List<Organization>> GetAllAsync()
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.GetAllAsync();
        }

        public List<Organization> GetAllFlat()
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();

            return orgManager.GetAll();
        }

        /// <summary>
        /// Gets organization by its ID
        /// </summary>
        /// <returns>Organization Object of entity objects</returns>
        public Organization GetByID(int organizationId)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.GetById(organizationId);
        }

        /// <summary>
        /// Gets organization by its name
        /// </summary>
        /// <returns>Organization Object of entity objects</returns>
        public Organization GetByName(string organizationName)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var facilityManager = this.container.GetInstance<OrganizationFacilityManager>();

            var org = organizationManager.GetByName(organizationName);

            org.OrganizationFacilities = facilityManager.GetByOrganization(org.Id);
            
            return org;
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of organization objects</returns>
        public Task<List<Organization>> SearchAsync(int? organizationId, int? facilityId)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.SearchAsync(organizationId, facilityId);
        }

        /// <summary>
        /// Gets all the records that match a contains search on any of the provided values
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of organization objects</returns>
        public List<Organization> Search(int? organizationId, int? facilityId)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.Search(organizationId, facilityId);
        }

        /// <summary>
        /// Get all the organization types
        /// </summary>
        /// <returns></returns>
        public Task<List<OrganizationType>> GetOrganizationTypesAsync()
        {
            var organizationTypeManager = this.container.GetInstance<OrganizationTypeManager>();

            return organizationTypeManager.GetAllAsync();
        }

        /// <summary>
        /// Get all Accredited Services
        /// </summary>
        /// <returns></returns>
        public string GetAccreditedServices(int organizationId)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            return organizationManager.GetAccreditedServices(organizationId);
        }

        /// <summary>
        /// Get all Accreditation Status
        /// </summary>
        /// <returns></returns>
        public Task<List<AccreditationStatus>> GetAccreditationStatusAsync()
        {
            var accreditationStatusManager = this.container.GetInstance<AccreditationStatusManager>();
            return accreditationStatusManager.GetAllAsync();
        }

        /// <summary>
        /// Get all BAA owner
        /// </summary>
        /// <returns></returns>
        public Task<List<BAAOwner>> GetBAAOwnerAsync()
        {
            var baaOwnerManager = this.container.GetInstance<BAAOwnerManager>();
            return baaOwnerManager.GetAllAsync();
        }

        /// <summary>
        /// Gets organization applications by its ID
        /// </summary>
        /// <returns>Collection of Application object</returns>
        public List<Application> GetOrganizationApplications(int organizationId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            return applicationManager.GetAllByOrganizationOrApplicationType(organizationId, null);
        }

        public List<SiteItems> GetOrganizationSites(string organizationName)
        {
            var siteManager = this.container.GetInstance<SiteManager>();
            var orgFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
            var strongRelations = orgFacilityManager.GetSiteIdsWithStrongRelations(organizationName);

            var sites = siteManager.GetSitesByOrganizationInclusive(organizationName);

            sites = sites.OrderBy(x=>Comparer.OrderSite(x, strongRelations)).ToList();

            var result = sites.Select(x=>ModelConversions.Convert(x, true, false)).ToList();

            foreach (var record in result)
            {
                record.Facilities = new List<FacilityItems>();
                var site = sites.Single(x => x.Id == record.SiteId);

                record.IsStrong = strongRelations.Any(x => x == record.SiteId);

                foreach (var fac in site.FacilitySites)
                {
                    record.Facilities.Add(ModelConversions.Convert(fac.Facility, false, true));
                }
            }

            return result;
        }

        public List<OrganizationBAADocument> GetOrganizationBAADocuments(int orgId)
        {
            var organizationBAADocumentManager = this.container.GetInstance<OrganizationBAADocumentManager>();

            var documentItem = organizationBAADocumentManager.GetByOrganization(orgId);

            return documentItem;
        }

        public List<SiteItems> GetOrganizationSitesByApplication(Guid applicationUniqueId)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var application = applicationManager.GetByUniqueId(applicationUniqueId);

            if (application == null)
            {
                return null;
            }

            var organization = organizationManager.GetById(application.OrganizationId);

            if (organization == null) return null;

            return (from orgFac in organization.OrganizationFacilities
                    from facSite in orgFac.Facility.FacilitySites
                    select ModelConversions.Convert(facSite.Site))
                .ToList();
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
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.SearchAsync(organizationName, city, state);
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
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.Search(organizationName, city, state);
        }

        /// <summary>
        /// Gets all the organizations by facility id asynchronously
        /// </summary>
        /// <param name="facilityId">Id of the facility</param>
        /// <returns>Collection of organization objects</returns>
        public Task<List<Organization>> GetByFacilityIdAndRelation(int facilityId, bool strongRelation)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            return organizationManager.GetByFacilityIdAndRelation(facilityId, strongRelation);
        }

        /// <summary>
        /// Saves new organization
        /// </summary>
        /// <param name="organizationItem"></param>
        /// <returns></returns>
        public async Task<Organization> SaveAsync(OrganizationItem organizationItem, string email, string ipAddress)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var cacheStatusManager = this.container.GetInstance<CacheStatusManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var userManager = this.container.GetInstance<UserManager>();
            // var organizationAddressManager = this.container.GetInstance<OrganizationAddressManager>();
            //var addressManager = this.container.GetInstance<AddressManager>();
            // var addressTypeManager = this.container.GetInstance<AddressTypeManager>();
            var organizationAccreditationCycleManager = this.container.GetInstance<OrganizationAccreditationCycleManager>();

            //var address = Convert(organizationItem.OrganizationAddressItem, email);
            //addressManager.Add(address);
            var userName = EncryptionHelper.GetFormattedText(organizationItem.OrganizationName);

            var groups = trueVaultManager.GetAllGroups();

            if (groups.Result != TrueVaultManager.Success)
            {
                throw new Exception("Cannot get True Vault Groups");
            }

            if (string.IsNullOrWhiteSpace(organizationItem.DocumentLibraryGroupId))
            {
                var trueVault = trueVaultManager.CreateOrganization(organizationItem.OrganizationName, userName);
                organizationItem.DocumentLibraryGroupId = trueVault.GroupId;
                organizationItem.DocumentLibraryVaultId = trueVault.VaultId;
            }

            var organization = organizationManager.Save(organizationItem, email);

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

            foreach (
                var director in
                organization.Users.Where(x => x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector))
            {
                trueVaultManager.AddUserToGroups(userOrgs, director.User.DocumentLibraryUserId, groups, organizationItem.DocumentLibraryGroupId, organizationItem.OrganizationName);
            }
            
            if (organizationItem.CycleEffectiveDate.HasValue)
            {
                organizationAccreditationCycleManager.RemoveCurrentAndAddCycle(organization.Id, organizationItem.CycleEffectiveDate.Value, email, organizationItem.CycleNumber);
            }

            var factStaff = userManager.GetFactStaff();

            foreach (var staff in factStaff)
            {
                trueVaultManager.AddUserToGroups(userOrgs, staff.DocumentLibraryUserId, groups, organizationItem.DocumentLibraryGroupId, organizationItem.OrganizationName);
            }

            await cacheStatusManager.UpdateCacheDateAsync(Constants.CacheStatuses.Organizations, email);

            //var organizationAddress = Convert(organization.Id, address.Id, email);
            //organizationAddressManager.Add(organizationAddress);
            return organization;
        }

        /// <summary>
        /// Updates existing organization
        /// </summary>
        /// <param name="organizationItem">instance of organization</param>
        /// <param name="email">user id/email of user updating the organization</param>
        /// <returns></returns>
        public async Task UpdateOrganizationAsync(OrganizationItem organizationItem, string email, string ipAddress)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var organizationAccreditationCycleManager = this.container.GetInstance<OrganizationAccreditationCycleManager>();
            var userOrganizationManager = this.container.GetInstance<UserOrganizationManager>();
            var cacheStatusManager = this.container.GetInstance<CacheStatusManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var userManager = this.container.GetInstance<UserManager>();

            this.CheckHistory(organizationItem, email);
            try
            {
                await organizationManager.UpdateOrganizationAsync(organizationItem, email);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            var groups = trueVaultManager.GetAllGroups();

            var userOrgs = new List<UserOrganizationItem>
                    {
                        new UserOrganizationItem
                        {
                            Organization = organizationItem
                        }
                    };

            userOrgs = organizationManager.FillTrueVaultGroupsForUsers(userOrgs);

            if (organizationItem.OrganizationDirectors != null)
            {
                foreach (var director in organizationItem.OrganizationDirectors)
                {
                    trueVaultManager.AddUserToGroups(userOrgs, director.DocumentLibraryUserId, groups);
                }
            }

            if (organizationItem.PrimaryUser.UserId != null)
            {
                var jobFunctionManager = this.container.GetInstance<JobFunctionManager>();

                var jobFunction = jobFunctionManager.GetByName(Constants.JobFunctions.OrganizationAdmin);

                var currentAdmin = await userOrganizationManager.GetByOrgAndJobFunction(organizationItem.OrganizationId, jobFunction.Id);

                if (currentAdmin == null || currentAdmin.UserId != organizationItem.PrimaryUser.UserId)
                {
                    if (currentAdmin != null && currentAdmin.UserId != organizationItem.PrimaryUser.UserId)
                    {
                        userOrganizationManager.RemoveRelation(currentAdmin.Id);
                    }

                    userOrganizationManager.AddRelation(organizationItem.OrganizationId, organizationItem.PrimaryUser.UserId, jobFunction.Id, true, email);
                }

                if (organizationItem.PrimaryUser.DocumentLibraryUserId == null)
                {
                    var primary = userManager.GetById(organizationItem.PrimaryUser.UserId.Value);
                    organizationItem.PrimaryUser.DocumentLibraryUserId = primary.DocumentLibraryUserId;
                }

                trueVaultManager.AddUserToGroups(userOrgs, organizationItem.PrimaryUser.DocumentLibraryUserId, groups);
            }

            if (!organizationItem.CycleNumber.HasValue) return;

            var cycle = await organizationAccreditationCycleManager.GetCurrentByOrganizationAsync(organizationItem.OrganizationId);

            if (cycle != null && cycle.Number != organizationItem.CycleNumber.Value)
            {
                await
                    organizationAccreditationCycleManager.RemoveCurrentAndAddCycleAsync(
                        organizationItem.OrganizationId, organizationItem.CycleEffectiveDate.GetValueOrDefault(), email);
            }

            //Constants.AccreditationsStatus.Withdrawn = 5 
            if (organizationItem.AccreditationStatusItem.Id == 5)
            {
                var applications = GetOrganizationApplications(organizationItem.OrganizationId);

                if (applications.Count > 0)
                {
                    var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
                    var applicationStatus = applicationStatusManager.GetByName(Constants.ApplicationStatus.Cancelled);

                    if (applicationStatus == null)
                        throw new ObjectNotFoundException(string.Format("Cannot find application status {0}", Constants.ApplicationStatus.Cancelled));

                    foreach (var app in applications)
                    {
                        var applicationManager = this.container.GetInstance<ApplicationManager>();
                        await applicationManager.UpdateApplicationStatusAsync(app, organizationItem.OrganizationId, app.ApplicationTypeId, applicationStatus.Id, Constants.ApplicationStatus.Cancelled, null, email);
                    }
                }
            }

            await cacheStatusManager.UpdateCacheDateAsync(Constants.CacheStatuses.Organizations, email);
        }

        private void CheckHistory(OrganizationItem organizationItem, string email)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            var organization = organizationManager.GetById(organizationItem.OrganizationId);

            DateTime? accredDate = null, expirationDate = null, extensionDate = null;
            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationDate))
            {
                accredDate = System.Convert.ToDateTime(organizationItem.AccreditationDate);
            }
            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationExpirationDate))
            {
                expirationDate = System.Convert.ToDateTime(organizationItem.AccreditationExpirationDate);
            }

            if (!string.IsNullOrWhiteSpace(organizationItem.AccreditationExtensionDate))
            {
                extensionDate = System.Convert.ToDateTime(organizationItem.AccreditationExtensionDate);
            }

            if (
                !Comparer.DoesNotMatch(organizationItem.AccreditationStatusItem?.Id, organization.AccreditationStatusId) &&
                !Comparer.DoesNotMatch(accredDate, organization.AccreditationDate) &&
                !Comparer.DoesNotMatch(expirationDate, organization.AccreditationExpirationDate) &&
                !Comparer.DoesNotMatch(extensionDate, organization.AccreditationExtensionDate)) return;


            var historyManager = this.container.GetInstance<OrganizationAccreditationHistoryManager>();

            var history = new OrganizationAccreditationHistory
            {
                Id = Guid.NewGuid(),
                OrganizationId = organization.Id,
                AccreditationStatusId = organization.AccreditationStatusId,
                AccreditationDate = organization.AccreditationDate,
                ExpirationDate = organization.AccreditationExpirationDate,
                ExtensionDate = organization.AccreditationExtensionDate,
                CreatedBy = email,
                CreatedDate = DateTime.Now
            };

            historyManager.Add(history);
        }

        /// <summary>
        /// Gets all organizations that have submitted eligibility applications
        /// </summary>
        /// <returns>Collection of Organizations</returns>
        public List<OrganizationItem> GetAllWithSubmittedEligibility()
        {
            var result = new List<OrganizationItem>();

            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var statuses = new List<string>
            {
                Constants.ApplicationStatus.Applied,
                Constants.ApplicationStatus.InReview,
                Constants.ApplicationStatus.ForReview,
                Constants.ApplicationStatus.RFI,
                Constants.ApplicationStatus.RFIReview,
                Constants.ApplicationStatus.ApplicantResponse,
                Constants.ApplicationStatus.ApplicantResponseReview
            };

            var orgs = organizationManager.GetAllWithSubmittedEligibility();

            foreach (var org in orgs)
            {
                var item = ModelConversions.Convert(org, false, false);

                //var applications = applicationManager.GetAllByOrganizationOrApplicationType(org.Id, null);

                var app = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Eligibility);

                if (app != null)
                {
                    item.EligibilityApplicationUniqueId = app.UniqueId;                  
                }
                var renewalApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Renewal);

                if (renewalApp != null)
                {
                    item.RenewalApplicationUniqueId = renewalApp.UniqueId;
                }

                var complienceApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && (x.ApplicationType.Name == Constants.ApplicationTypes.CT || x.ApplicationType.Name == Constants.ApplicationTypes.CordBlood));

                if (complienceApp != null)
                {
                    item.ComplianceApplicationUniqueId = complienceApp.ComplianceApplicationId;
                    item.ApplicationUniqueId = complienceApp.UniqueId;
                }

                result.Add(item);
            }


            return result;
        }

        /// <summary>
        /// Gets all organizations that have submitted eligibility applications asynchronously
        /// </summary>
        /// <returns>Collection of Organizations</returns>
        public async Task<List<OrganizationItem>> GetAllWithSubmittedEligibilityAsync()
        {
            var result = new List<OrganizationItem>();

            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var statuses = new List<string>
            {
                Constants.ApplicationStatus.Applied,
                Constants.ApplicationStatus.InReview,
                Constants.ApplicationStatus.ForReview,
                Constants.ApplicationStatus.RFI,
                Constants.ApplicationStatus.RFIReview,
                Constants.ApplicationStatus.ApplicantResponse,
                Constants.ApplicationStatus.ApplicantResponseReview
            };

            var orgs = await organizationManager.GetAllWithSubmittedEligibilityAsync();

            foreach (var org in orgs)
            {
                var item = ModelConversions.Convert(org, false, false);

                var applications = await applicationManager.GetAllByOrganizationOrApplicationTypeAsync(org.Id, null);

                var app = applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Eligibility);

                if (app != null)
                {
                    item.EligibilityApplicationUniqueId = app.UniqueId;
                    item.ComplianceApplicationUniqueId = app.ComplianceApplicationId;
                }
                var renewalApp = applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Renewal);

                if (renewalApp != null)
                {
                    item.RenewalApplicationUniqueId = renewalApp.UniqueId;
                }

                var complienceApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && (x.ApplicationType.Name == Constants.ApplicationTypes.CT || x.ApplicationType.Name == Constants.ApplicationTypes.CordBlood));

                if (complienceApp != null)
                {
                    item.ComplianceApplicationUniqueId = complienceApp.ComplianceApplicationId;
                    item.ApplicationUniqueId = complienceApp.UniqueId;
                }


                result.Add(item);
            }


            return result;
        }

        public List<NetcordMembershipType> GetNetcordMembershipTypes()
        {
            var manager = this.container.GetInstance<NetcordMembershipTypeManager>();

            return manager.GetAll();
        }

        public Task<List<NetcordMembershipType>> GetNetcordMembershipTypesAsync()
        {
            var manager = this.container.GetInstance<NetcordMembershipTypeManager>();

            return manager.GetAllAsync();
        }

        public void SetDocumentLibrary()
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

            var orgs = orgManager.GetOrganizationsWithoutDocumentLibrary();

            foreach (var org in orgs)
            {
                var userName = EncryptionHelper.GetFormattedText(org.Name);

                var trueVault = trueVaultManager.CreateOrganization(org.Name, userName);

                org.DocumentLibraryGroupId = trueVault.GroupId;
                org.DocumentLibraryVaultId = trueVault.VaultId;
                org.UpdatedDate = DateTime.Now;
                org.UpdatedBy = "System";

                orgManager.BatchSave(org);
            }

            if (orgs.Count > 0)
            {
                orgManager.SaveChanges();
            }
        }

        public void SetDocumentLibraryGroupIds()
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

            var orgs = orgManager.GetOrganizationsWithoutDocumentLibrary();

            var groups = trueVaultManager.GetAllGroups();

            if (groups == null || groups.Result != TrueVaultManager.Success)
            {
                throw new Exception("Cannot get groups");
            }

            foreach (var org in orgs)
            {
                var groupName = org.Name;

                if (groupName.Length > 75)
                {
                    groupName = org.Name.Substring(0, 75);
                }

                var group = groups.Groups.SingleOrDefault(x => x.Name == groupName);

                if (group == null) continue;

                org.DocumentLibraryGroupId = group.Group_id;

                orgManager.Save(org);
            }
        }

        public async Task SetDocumentLibraryAsync()
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

            var orgs = orgManager.GetOrganizationsWithoutDocumentLibrary();

            foreach (var org in orgs)
            {
                var userName = EncryptionHelper.GetFormattedText(org.Name);

                var trueVault = trueVaultManager.CreateOrganization(org.Name, userName);

                org.DocumentLibraryGroupId = trueVault.GroupId;
                org.DocumentLibraryVaultId = trueVault.VaultId;
                org.UpdatedDate = DateTime.Now;
                org.UpdatedBy = "System";

                orgManager.BatchSave(org);
            }

            if (orgs.Count > 0)
            {
                await orgManager.SaveChangesAsync();
            }
        }

        public List<SimpleOrganization> GetSimpleOrganizations()
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();

            return orgManager.GetSimpleOrganizations();
        }
    }
}
