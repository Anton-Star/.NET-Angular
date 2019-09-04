using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(FactWebContext context) : base(context)
        {
        }

        public List<Facility> GetAllFacilities()
        {
            return
                base.Context.Facilities
                    .Include(x => x.FacilityAccreditationMapping)
                    .Include(x=>x.FacilityAccreditationMapping.Select(y=>y.FacilityAccreditation))
                    .Include(x => x.FacilitySites)
                    .Include(x => x.FacilitySites.Select(y => y.Site))
                    .Include(x => x.FacilitySites.Select(y => y.Site.InspectionScheduleSites))
                    .Include(x=>x.FacilitySites.Select(y=>y.Site.SiteClinicalTypes))
                    .Include(x=>x.FacilityDirector)
                    .Include(x=>x.PrimaryOrganization)
                    .Include(x=>x.PrimaryOrganization.OrganizationType)
                    .Include(x=>x.MasterServiceType)
                    .ToList();
        }

        public Facility GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<Facility> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }

        /// <summary>
        /// Get all active facilities asynchronously
        /// </summary>
        public Task<List<Facility>> GetAllActiveAsync()
        {
            return base.FetchManyAsync(x => x.IsActive == true);
        }

        /// <summary>
        /// Get all active facilitie
        /// </summary>
        public List<Facility> GetAllActive()
        {
            var sites = base.Context.Site
                .Include(x => x.InspectionScheduleSites)
                .Include(x => x.SiteClinicalTypes)
                .Include(x => x.SiteProcessingTypes)
                .Include(x => x.SiteAddresses)
                .Include(x => x.SiteAddresses.Select(y => y.Address))
                .ToList();

            var facilities =
                base.Context.Facilities
                .Where(x=>x.IsActive)
                    .Include(x => x.FacilityAccreditationMapping)
                    .Include(x => x.FacilityAccreditationMapping.Select(y => y.FacilityAccreditation))
                    .Include(x => x.FacilitySites)
                    .Include(x => x.FacilityDirector)
                    .Include(x => x.PrimaryOrganization)
                    .Include(x => x.PrimaryOrganization.OrganizationType)
                    .Include(x => x.MasterServiceType)
                    .ToList();

            foreach (var facility in facilities)
            {
                foreach (var facSite in facility.FacilitySites)
                {
                    facSite.Site = sites.SingleOrDefault(x => x.Id == facSite.SiteId);
                }
            }

            return facilities;
        }

        public Task<string> GetCBCollectionSiteTypes(int facilityId)
        {   

            var accreditedServicesList = base.Context.Facilities
                .Where(x => x.Id == facilityId)
                            .Select(x => x.FacilitySites.Select(y => y.Facility.ServiceType.Name).Distinct()).ToList();

            string cbCollectionSiteTypes = string.Empty;

            if (accreditedServicesList.Count > 0)
            {
                string[] typeList = accreditedServicesList[0].ToArray();
                cbCollectionSiteTypes = string.Join(", ", typeList);
            }

            return Task.FromResult(cbCollectionSiteTypes);
            
        }

        public List<Facility> GetAllByOrg(int organizationId)
        {
            return
                base.Context.Facilities
                    .Where(x => x.IsActive && x.OrganizationFacilities.Any(y => y.OrganizationId == organizationId))
                    .Include(x => x.FacilityAccreditationMapping)
                    .Include(x => x.FacilityAccreditationMapping.Select(y => y.FacilityAccreditation))
                    .Include(x => x.FacilitySites)
                    .Include(x => x.FacilitySites.Select(y => y.Site))
                    .Include(x => x.FacilitySites.Select(y => y.Site.InspectionScheduleSites))
                    .Include(x => x.FacilitySites.Select(y => y.Site.SiteClinicalTypes))
                    .Include(x => x.FacilityDirector)
                    .Include(x => x.PrimaryOrganization)
                    .Include(x => x.PrimaryOrganization.OrganizationType)
                    .Include(x => x.MasterServiceType)
                    .ToList();
        }
    }
}
