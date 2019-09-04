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
    public class SiteRepository : BaseRepository<Site>, ISiteRepository
    {
        public SiteRepository(FactWebContext context) : base(context)
        {
        }

        public List<Site> GetAllSites()
        {
            return
                base.Context.Site
                    .Include(x => x.CBCollectionType)
                    .Include(x => x.CBUnitType)
                    .Include(x => x.ClinicalPopulationType)
                    .Include(x => x.CollectionProductType)
                    .Include(x => x.FacilitySites)
                    .Include(x => x.FacilitySites.Select(y => y.Facility))
                    .Include(x=>x.FacilitySites.Select(y=>y.Facility.ServiceType))
                    .Include(x => x.FacilitySites.Select(y => y.Facility.MasterServiceType))
                    //.Include(x => x.Country)
                    .Include(x => x.InspectionScheduleSites)
                    .Include(x => x.InspectionScheduleSites.Select(y => y.InspectionSchedule))
                    .Include(x => x.InspectionScheduleSites.Select(y => y.InspectionSchedule.InspectionScheduleDetails))
                    .Include(x => x.SiteAddresses)
                    .Include(x => x.SiteAddresses.Select(y => y.Address))
                    .Include(x => x.SiteAddresses.Select(y => y.Address.Country))
                    .Include(x => x.SiteAddresses.Select(y => y.Address.State))
                    .Include(x => x.SiteClinicalTypes)
                    .Include(x => x.SiteClinicalTypes.Select(y => y.ClinicalType))
                    .Include(x => x.SiteProcessingTypes)
                    .Include(x => x.SiteProcessingTypes.Select(y => y.ProcessingType))
                    .Include(x => x.SiteCollectionTotals)
                    .Include(x => x.SiteCollectionTotals.Select(y => y.ClinicalPopulationType))
                    .Include(x => x.SiteCollectionTotals.Select(y => y.CollectionType))
                    .Include(x => x.SiteCordBloodTransplantTotals)
                    .Include(x => x.SiteCordBloodTransplantTotals.Select(y => y.CbCategory))
                    .Include(x => x.SiteCordBloodTransplantTotals.Select(y => y.CbUnitType))
                    .Include(x => x.SiteProcessingMethodologyTotals)
                    .Include(x => x.SiteProcessingMethodologyTotals.Select(y => y.ProcessingType))
                    .Include(x => x.SiteProcessingTotals)
                    .Include(x => x.SiteProcessingTotals.Select(y => y.TransplantCellType))
                    .Include(x => x.SiteProcessingTotals.Select(y => y.SiteProcessingTotalTransplantCellTypes))
                    .Include(
                        x =>
                            x.SiteProcessingTotals.Select(
                                y => y.SiteProcessingTotalTransplantCellTypes.Select(z => z.TransplantCellType)))
                    .Include(
                        x =>
                            x.SiteProcessingTotals.Select(
                                y => y.SiteProcessingTotalTransplantCellTypes.Select(z => z.SiteProcessingTotal)))
                    .Include(x => x.SiteScopeTypes)
                    .Include(x => x.SiteScopeTypes.Select(y => y.ScopeType))
                    .Include(x => x.SiteTransplantTotals)
                    .Include(x => x.SiteTransplantTotals.Select(y => y.ClinicalPopulationType))
                    .Include(x => x.SiteTransplantTotals.Select(y => y.TransplantCellType))
                    .Include(x => x.SiteTransplantTotals.Select(y => y.TransplantType))
                    .Include(x => x.SiteTransplantTypes)
                    .Include(x => x.SiteTransplantTypes.Select(y => y.TransplantType))
                    .ToList();
        }

        public Task<List<Site>> SearchAsync(string organizationName)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.Name == organizationName);
        }

        public List<Site> Search(string organizationName)
        {
            return
                base.FetchMany(
                    x =>
                        x.Name == organizationName);
        }

        public Site GetByName(string siteName)
        {
            return base.Fetch(x => x.Name == siteName);
        }

        public Task<Site> GetByNameAsync(string siteName)
        {
            return base.FetchAsync(x => x.Name == siteName);
        }

        public List<SiteScopeType> GetScopeTypes(long organizationId)
        {
            var scopeTypesList = base.Context.SiteScopeType
                            .Where(x => x.Site.FacilitySites.Any(y => y.Facility.OrganizationFacilities.Any(z => z.OrganizationId == organizationId))).ToList();

            return scopeTypesList;
        }

        public List<SiteScopeType> GetScopeTypesBySite(int siteId)
        {
            var scopeTypesList = base.Context.SiteScopeType
                            .Where(x => x.SiteId == siteId).ToList();

            return scopeTypesList;
        }

        public List<Site> GetSitesByOrganizationId(long organizationId)
        {
            var sitesList = base.Context.FacilitySite.Where(y => y.Facility.OrganizationFacilities.Any(z => z.OrganizationId == organizationId)).Select(x => x.Site).ToList();
            //.Where(x => x.Site.FacilitySites.Any(y => y.Facility.OrganizationFacilities.Any(z => z.OrganizationId == organizationId))).ToList();

            return sitesList;
        }

        

        public Site GetPrimarySite(long organizationId)
        {
            var site = base.Context.Site.Where(x => x.IsPrimarySite && x.FacilitySites.Any(y => y.Facility.OrganizationFacilities.Any(z => z.OrganizationId == organizationId && z.StrongRelation))).FirstOrDefault();

            return site;
        }

        public List<FlatSite> GetFlatSites()
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var data = objectContext.ExecuteStoreQuery<FlatSite>(
                "EXEC usp_getSites").ToList();

            return data;
        }

        public List<Site> GetSitesByOrganizationInclusive(string orgName)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return
                base.Context.Site
                    .Include(x => x.CBCollectionType)
                    .Include(x => x.CBUnitType)
                    .Include(x => x.ClinicalPopulationType)
                    .Include(x => x.CollectionProductType)
                    //.Include(x => x.Country)
                    .Include(x => x.SiteAddresses)
                    .Include(x => x.SiteAddresses.Select(y => y.Address))
                    .Include(x => x.SiteAddresses.Select(y => y.Address.Country))
                    .Include(x => x.SiteAddresses.Select(y => y.Address.State))
                    .Include(x => x.SiteClinicalTypes)
                    .Include(x => x.SiteClinicalTypes.Select(y => y.ClinicalType))
                    .Include(x => x.SiteProcessingTypes)
                    .Include(x => x.SiteProcessingTypes.Select(y => y.ProcessingType))
                    .Include(x => x.SiteScopeTypes)
                    .Include(x => x.SiteScopeTypes.Select(y => y.ScopeType))
                    .Include(x => x.SiteTransplantTypes)
                    .Include(x => x.SiteTransplantTypes.Select(y => y.TransplantType))
                    .Include(x=>x.FacilitySites.Select(y=>y.Facility))
                    .Include(x => x.FacilitySites.Select(y => y.Facility.ServiceType))
                    .Where(
                        x =>
                            x.FacilitySites.Any(
                                y => y.Facility.OrganizationFacilities.Any(z => z.Organization.Name == orgName)))
                    .ToList();
        }

        public List<Site> GetSitesByComplianceNoFacility(Guid complianceApplicationId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return
                base.Context.Site
                    .Include(x => x.CBCollectionType)
                    .Include(x => x.CBUnitType)
                    .Include(x => x.ClinicalPopulationType)
                    .Include(x => x.CollectionProductType)
                    .Include(x => x.SiteAddresses)
                    .Include(x => x.SiteAddresses.Select(y => y.Address))
                    .Include(x => x.SiteAddresses.Select(y => y.Address.Country))
                    .Include(x => x.SiteAddresses.Select(y => y.Address.State))
                    .Include(x => x.SiteScopeTypes)
                    .Include(x => x.SiteScopeTypes.Select(y => y.ScopeType))
                    .Include(x => x.InspectionScheduleSites)
                    .Where(x => x.Applications.Any(y => y.ComplianceApplicationId == complianceApplicationId))
                    .ToList();
        }
    }
}
