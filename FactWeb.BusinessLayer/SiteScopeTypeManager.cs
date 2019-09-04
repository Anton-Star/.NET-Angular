using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactWeb.BusinessLayer
{
    public class SiteScopeTypeManager : BaseManager<SiteScopeTypeManager, ISiteScopeTypeRepository, SiteScopeType>
    {
        public SiteScopeTypeManager(ISiteScopeTypeRepository repository) : base(repository)
        {
        }

        public void AddorUpdateSiteScopeType(SiteItems siteItem, int siteId, string updatedBy)
        {
            LogMessage("AddorUpdateSiteScopeType (SiteScopeTypeManager)");

            var existingSiteScopeTypes = base.Repository.GetAllBySiteId(siteId);

            foreach (var item in existingSiteScopeTypes)
            {
                base.BatchRemove(item);
            }

            var newSiteScopeTypes = siteItem.ScopeTypes.Where(x => x.IsSelected == true).ToList();

            foreach (var objCredential in newSiteScopeTypes)
            {
                SiteScopeType siteScopeType = new SiteScopeType();
                siteScopeType.Id = Guid.NewGuid();
                siteScopeType.ScopeTypeId = objCredential.ScopeTypeId;
                siteScopeType.SiteId = siteId;
                siteScopeType.CreatedBy = updatedBy;
                siteScopeType.CreatedDate = DateTime.Now;
                siteScopeType.UpdatedBy = updatedBy;
                siteScopeType.UpdatedDate = DateTime.Now;

                base.BatchAdd(siteScopeType);
            }

            base.Repository.SaveChanges();
        }

        public List<SiteScopeType> GetBySite(int siteId)
        {
            return this.Repository.GetAllBySiteId(siteId);
        }
    }
}
