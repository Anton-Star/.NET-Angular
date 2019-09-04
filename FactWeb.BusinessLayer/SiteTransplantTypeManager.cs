using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Linq;

namespace FactWeb.BusinessLayer
{
    public class SiteTransplantTypeManager : BaseManager<SiteTransplantTypeManager, ISiteTransplantTypeRepository, SiteTransplantType>
    {
        public SiteTransplantTypeManager(ISiteTransplantTypeRepository repository) : base(repository)
        {
        }

        public void AddorUpdateSiteTransplantType(SiteItems siteItem, int siteId, string updatedBy)
        {
            LogMessage("AddorUpdateSiteTransplantType (SiteTransplantTypeManager)");

            var existingSiteTransplantTypes = base.Repository.GetAllBySiteId(siteId);

            foreach (var item in existingSiteTransplantTypes)
            {
                base.BatchRemove(item);
            }

            var newSiteTransplantTypes = siteItem.TransplantTypes.Where(x => x.IsSelected == true).ToList();

            foreach (var objTransplantType in newSiteTransplantTypes)
            {
                SiteTransplantType siteTransplantType = new SiteTransplantType();
                siteTransplantType.Id = Guid.NewGuid();
                siteTransplantType.TransplantTypeId = objTransplantType.Id;
                siteTransplantType.SiteId = siteId;
                siteTransplantType.CreatedBy = updatedBy;
                siteTransplantType.CreatedDate = DateTime.Now;
                siteTransplantType.UpdatedBy = updatedBy;
                siteTransplantType.UpdatedDate = DateTime.Now;

                base.BatchAdd(siteTransplantType);
            }

            base.Repository.SaveChanges();
        }
    }
}
