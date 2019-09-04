namespace FactWeb.Repository
{
    //public class SiteApplicationVersionRepository : BaseRepository<SiteApplicationVersion>, ISiteApplicationVersionRepository
    //{
    //    public SiteApplicationVersionRepository(FactWebContext context) : base(context)
    //    {
    //    }

    //    public List<SiteApplicationVersion> GetByOrganization(int organizationId)
    //    {
    //        return base.FetchMany(x => x.Application.OrganizationId == organizationId);
    //    }

    //    public Task<List<SiteApplicationVersion>> GetByOrganizationAsync(int organizationId)
    //    {
    //        return base.FetchManyAsync(x => x.Application.OrganizationId == organizationId);
    //    }

    //    public List<SiteApplicationVersion> GetComplianceByOrganization(int organizationId)
    //    {
    //        return
    //            base.FetchMany(
    //                x =>
    //                    x.Application.OrganizationId == organizationId &&
    //                    (x.ApplicationVersion.ApplicationType.Name == Constants.ApplicationTypes.CT ||
    //                     x.ApplicationVersion.ApplicationType.Name == Constants.ApplicationTypes.Common ||
    //                     x.ApplicationVersion.ApplicationType.Name == Constants.ApplicationTypes.CordBlood));
    //    }

    //    public Task<List<SiteApplicationVersion>> GetComplianceByOrganizationAsync(int organizationId)
    //    {
    //        return
    //            base.FetchManyAsync(
    //                x =>
    //                    x.Application.OrganizationId == organizationId &&
    //                    (x.ApplicationVersion.ApplicationType.Name == Constants.ApplicationTypes.CT ||
    //                     x.ApplicationVersion.ApplicationType.Name == Constants.ApplicationTypes.Common ||
    //                     x.ApplicationVersion.ApplicationType.Name == Constants.ApplicationTypes.CordBlood));
    //    }

    //    public List<SiteApplicationVersion> GetByComplianceApplication(int organizationId)
    //    {
    //        return
    //            base.FetchMany(
    //                x => x.Application.ComplianceApplication.OrganizationId == organizationId && x.Application.ComplianceApplication.IsActive);
    //    }

    //    public Task<List<SiteApplicationVersion>> GetByComplianceApplicationAsync(int organizationId)
    //    {
    //        return
    //            base.FetchManyAsync(
    //                x => x.Application.ComplianceApplication.OrganizationId == organizationId && x.Application.ComplianceApplication.IsActive);
    //    }
    //}
}
