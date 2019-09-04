namespace FactWeb.BusinessLayer
{
    //public class SiteApplicationVersionQuestionNotApplicableManager : BaseManager<ISiteApplicationVersionQuestionNotApplicableRepository, SiteApplicationVersionQuestionNotApplicable>
    //{
    //    public SiteApplicationVersionQuestionNotApplicableManager(ISiteApplicationVersionQuestionNotApplicableRepository repository) : base(repository)
    //    {
    //    }

    //    public List<SiteApplicationVersionQuestionNotApplicable> GetByOrganization(int organizationId)
    //    {
    //        return base.Repository.GetByOrganization(organizationId);
    //    }

    //    public Task<List<SiteApplicationVersionQuestionNotApplicable>> GetByOrganizationAsync(int organizationId)
    //    {
    //        return base.Repository.GetByOrganizationAsync(organizationId);
    //    }

    //    public void RemoveComplianceForOrganization(int organizationId)
    //    {
    //        var items = this.GetByOrganization(organizationId);

    //        foreach (var item in items)
    //        {
    //            base.Repository.BatchRemove(item);
    //        }

    //        base.Repository.SaveChanges();
    //    }

    //    public async Task RemoveComplianceForOrganizationAsync(int organizationId)
    //    {
    //        var items = await this.GetByOrganizationAsync(organizationId);

    //        foreach (var item in items)
    //        {
    //            base.Repository.BatchRemove(item);
    //        }

    //        await base.Repository.SaveChangesAsync();
    //    }
    //}
}
