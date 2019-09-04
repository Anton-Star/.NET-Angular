using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(FactWebContext context) : base(context)
        {
        }

        public override Organization GetById(int id)
        {
            return
                base.Context.Organizations
                    .Include(x => x.AccreditationStatus)
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.ServiceType))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.FacilityDirector))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.MasterServiceType))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site)))
                    .Include(
                        x =>
                            x.OrganizationFacilities.Select(
                                y => y.Facility.FacilitySites.Select(z => z.Site.InspectionScheduleSites)))
                    .Include(x => x.Applications)
                    .Include(x => x.Documents)
                    .Include(x => x.OrganizationAccreditationCycles)
                    .Include(x => x.OrganizationAccreditationHistories)
                    .Include(x => x.OrganizationType)
                    .Include(x => x.Users)
                    .Include(x => x.OrganizationAddresses)
                    .Include(x => x.OrganizationConsutants)
                    .Include(x => x.Users.Select(y=>y.User))
                    .Include(x => x.Users.Select(y => y.User.Role))
                    //.Include(x=>x.OrganizationNetcordMemberships)
                    .Include(x => x.PrimaryUser)
                    .Include(x => x.PrimaryUser.Role)
                    .Include(x => x.BAAOwner)
                    .Include(x => x.OrganizationType)
                    .Include(x => x.OrganizationAccreditationCycles)
                    .SingleOrDefault(x=>x.Id== id);
        }

        public List<Organization> GetAllOrganizations(bool includeFacilities)
        {
            var data = base.Context.Organizations
                .Include(x => x.AccreditationStatus)
                .Include(x=>x.Applications.Select(y=>y.ApplicationStatus))
                .Include(x => x.Applications.Select(y => y.ApplicationType))
                .Include(x => x.Documents)
                .Include(x => x.OrganizationAccreditationCycles)
                .Include(x => x.OrganizationAccreditationHistories)
                .Include(x => x.OrganizationType)
                .Include(x => x.Users)
                .Include(x => x.OrganizationAddresses)
                .Include(x => x.OrganizationConsutants)
                .Include(x => x.Users.Select(y => y.User))
                .Include(x => x.Users.Select(y => y.User.Role))
                //.Include(x=>x.OrganizationNetcordMemberships)
                .Include(x => x.PrimaryUser)
                .Include(x => x.PrimaryUser.Role)
                .Include(x => x.BAAOwner)
                .Include(x => x.OrganizationType)
                .Include(x => x.OrganizationAccreditationCycles);

            if (includeFacilities)
            {
                data.Include(x => x.OrganizationFacilities.Select(y => y.Facility.ServiceType))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.MasterServiceType))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site)))
                    .Include(
                        x =>
                            x.OrganizationFacilities.Select(
                                y => y.Facility.FacilitySites.Select(z => z.Site.InspectionScheduleSites)));
            }

            return data.ToList();
        }

        public List<Organization> GetAllOrganizationWithApplications()
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            var organizations = base.Context.Organizations
                .Include(x => x.AccreditationStatus)
                .Include(x => x.Documents)
                .Include(x => x.OrganizationType)
                .ToList();

            //var orgIds = organizations.Select(x => x.Id).ToList();

            var applications = base.Context.Applications
                .Include(x=>x.ApplicationStatus)
                .Include(x=>x.ApplicationType)
                .Where(x=>x.IsActive == true)
                //.Where(x => orgIds.Any(y => y == x.OrganizationId))
                .ToList();

            foreach (var org in organizations)
            {
                org.Applications = applications.Where(x => x.OrganizationId == org.Id).ToList();
            }

            return organizations;
        }

        public Task<List<Organization>> SearchAsync(string organizationName, string city, string state)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.Name == organizationName ||
                        x.OrganizationAddresses.Any(
                            y => (y.Address.City.Contains(city) && !string.IsNullOrEmpty(city)) || (y.Address.State.Id.ToString().Contains(state) && !string.IsNullOrEmpty(state))));
        }

        public List<Organization> Search(string organizationName, string city, string state)
        {
            return
                base.FetchMany(
                    x =>
                        x.Name == organizationName ||
                        x.OrganizationAddresses.Any(
                            y => (y.Address.City.Contains(city) && !string.IsNullOrEmpty(city)) || (y.Address.State.Id.ToString().Contains(state) && !string.IsNullOrEmpty(state))));
        }

        public List<Organization> GetByFacility(int facilityId)
        {
            return base.FetchMany(x => x.OrganizationFacilities.Any(y => y.FacilityId == facilityId));
        }

        public Task<List<Organization>> GetByFacilityAsync(int facilityId)
        {
            return base.FetchManyAsync(x => x.OrganizationFacilities.Any(y => y.FacilityId == facilityId));
        }

        public Task<List<Organization>> GetByFacilityIdAndRelationAsync(int facilityId, bool strongRelation)
        {
            return base.FetchManyAsync(x => x.OrganizationFacilities.Any(y => y.FacilityId == facilityId && y.StrongRelation == strongRelation));
        }

        public Organization GetByName(string organizationName)
        {
            return
                base.Context.Organizations
                    .Include(x => x.OrganizationConsutants)
                    .Include(x=>x.OrganizationType)
                    .SingleOrDefault(x => x.Name == organizationName);
        }
            
        public Task<Organization> GetByNameAsync(string organizationName)
        {
            return base.FetchAsync(x => x.Name == organizationName);
        }

        public string GetAccreditedServices(int organizationId)
        {
            var accreditedServicesList = base.Context.Organizations
                            .Where(x => x.Id == organizationId)
                            .Select(x => x.OrganizationFacilities.Select(y => y.Facility.ServiceType.Name).Distinct()).ToList();

            string strAccreditedServices = string.Empty;

            if (accreditedServicesList.Count > 0)
            {
                string[] tempList = accreditedServicesList[0].ToArray();
                strAccreditedServices = string.Join(", ", tempList);
            }

            return strAccreditedServices;
        }

        public List<Organization> GetAllWithSubmittedEligibility()
        {
            return
                base.Context.Organizations
                    .Where(x => x.Applications.Any(y => y.ApplicationType.Name == Constants.ApplicationTypes.Eligibility || y.ApplicationType.Name == Constants.ApplicationTypes.Renewal))
                    .Include(x => x.AccreditationStatus)
                    .Include(x => x.OrganizationFacilities)
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.FacilitySites))
                    .Include(x => x.OrganizationFacilities.Select(y => y.Facility.FacilitySites.Select(z => z.Site)))
                    .Include(
                        x =>
                            x.OrganizationFacilities.Select(
                                y => y.Facility.FacilitySites.Select(z => z.Site.InspectionScheduleSites)))
                    .Include(x => x.Applications)
                    .Include(x=>x.Applications.Select(y=>y.ApplicationVersion))
                    .Include(x=>x.Applications.Select(y=>y.ApplicationVersion.ApplicationSections))
                    .Include(x=>x.Applications.Select(y=>y.ApplicationType))
                    .Include(x => x.Documents)
                    .Include(x => x.OrganizationAccreditationCycles)
                    .Include(x => x.OrganizationAccreditationHistories)
                    .Include(x => x.OrganizationType)
                    .Include(x => x.Users)
                    .Include(x => x.OrganizationAddresses)
                    .Include(x => x.OrganizationConsutants)
                    .Include(x => x.Users.Select(y => y.User))
                    .Include(x => x.Users.Select(y => y.User.Role))
                    .Include(x => x.PrimaryUser)
                    .Include(x => x.PrimaryUser.Role)
                    .Include(x => x.BAAOwner)
                    .Include(x => x.OrganizationType)
                    .Include(x => x.OrganizationAccreditationCycles)
                    .ToList();
        }

        public Task<List<Organization>> GetAllWithSubmittedEligibilityAsync()
        {
            return
                base.FetchManyAsync(
                    x => x.Applications.Any(y => y.ApplicationType.Name == Constants.ApplicationTypes.Eligibility || y.ApplicationType.Name == Constants.ApplicationTypes.Renewal));
        }

        public bool DoesHaveAccess(string orgName, Guid userId)
        {
            return
                base.Context.Organizations.Any(
                    x =>
                        x.Name == orgName &&
                        x.Applications.Any(
                            y =>
                                y.InspectionSchedules.Any(
                                    z => z.InspectionScheduleDetails.Any(zz => zz.UserId == userId))));
        }

        public List<Organization> GetOrganizationsWithoutDocumentLibrary()
        {
            return base.FetchMany(x => x.DocumentLibraryGroupId == null);
        }

        public Task<List<Organization>> GetOrganizationsWithoutDocumentLibraryAsync()
        {
            return base.FetchManyAsync(x => x.DocumentLibraryVaultId == null);
        }

        public bool HasQmRestriction(int organizationId)
        {
            return base.Context.Organizations
                .Any(x => x.Id == organizationId && x.OrganizationFacilities.Any(y => y.Facility.QMRestrictions));
        }

        public void CreateDocumentLibrary(int organizationId, int cycleNumber, string vaultId, string groupId, string createdBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[5];

            paramList[0] = organizationId;
            paramList[1] = cycleNumber;
            paramList[2] = vaultId;
            paramList[3] = groupId;
            paramList[4] = createdBy;


            objectContext.ExecuteStoreQuery<CoordinatorApplication>(
                "EXEC usp_createDocumentLibrary @organizationId={0}, @cycleNumber={1}, @vaultId={2}, @groupId={3}, @createdBy={4}",
                paramList);
        }

        public List<SimpleOrganization> GetSimpleOrganizations()
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[0];

            return objectContext.ExecuteStoreQuery<SimpleOrganization>(
                "EXEC usp_getSimpleOrganizations",
                paramList).ToList();
        }

        public Organization GetByCompAppId(Guid compAppId)
        {
            var compApp = base.Context.ComplianceApplications
                .Include(x => x.Organization)
                .FirstOrDefault(x => x.Id == compAppId);

            return compApp?.Organization;
        }
    }
}
