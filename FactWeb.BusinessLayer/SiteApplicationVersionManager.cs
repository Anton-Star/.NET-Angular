namespace FactWeb.BusinessLayer
{
    //public class SiteApplicationVersionManager : BaseManager<ISiteApplicationVersionRepository, SiteApplicationVersion>
    //{
    //    public SiteApplicationVersionManager(ISiteApplicationVersionRepository repository) : base(repository)
    //    {
    //    }

    //    public List<SiteApplicationVersion> GetByOrganization(int organizationId)
    //    {
    //        return base.Repository.GetByOrganization(organizationId);
    //    }

    //    public Task<List<SiteApplicationVersion>> GetByOrganizationAsync(int organizationId)
    //    {
    //        return base.Repository.GetByOrganizationAsync(organizationId);
    //    }

    //    public List<SiteApplicationVersion> GetComplianceByOrganization(int organizationId)
    //    {
    //        return base.Repository.GetComplianceByOrganization(organizationId);
    //    }

    //    public Task<List<SiteApplicationVersion>> GetComplianceByOrganizationAsync(int organizationId)
    //    {
    //        return base.Repository.GetComplianceByOrganizationAsync(organizationId);
    //    }

    //    public List<SiteApplicationVersion> GetByComplianceApplication(int organizationId)
    //    {
    //        return base.Repository.GetByComplianceApplication(organizationId);
    //    }

    //    public Task<List<SiteApplicationVersion>> GetByComplianceApplicationAsync(int organizationId)
    //    {
    //        return base.Repository.GetByComplianceApplicationAsync(organizationId);
    //    }

    //    public void RemoveComplianceItems(int organizationId)
    //    {
    //        var items = this.GetByComplianceApplication(organizationId);

    //        foreach (var item in items)
    //        {
    //            base.Repository.BatchRemove(item);
    //        }

    //        base.Repository.SaveChanges();
    //    }

    //    public async Task RemoveComplianceItemsAsync(int organizationId)
    //    {
    //        var items = this.GetByComplianceApplication(organizationId);

    //        foreach (var item in items)
    //        {
    //            base.Repository.BatchRemove(item);
    //        }

    //        await base.Repository.SaveChangesAsync();
    //    }
    //}
}
