using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Linq;

namespace FactWeb.BusinessLayer
{
    public class SiteProcessingTypeManager : BaseManager<SiteProcessingTypeManager, ISiteProcessingTypeRepository, SiteProcessingType>
    {
        public SiteProcessingTypeManager(ISiteProcessingTypeRepository repository) : base(repository)
        {
        }

        public void AddorUpdateSiteProcessingType(SiteItems siteItem, int siteId, string updatedBy)
        {
            LogMessage("AddorUpdateSiteProcessingType (SiteProcessingTypeManager)");

            var existingSiteProcessingTypes = base.Repository.GetAllBySiteId(siteId);

            foreach (var item in existingSiteProcessingTypes)
            {
                base.BatchRemove(item);
            }

            var newSiteProcessingTypes = siteItem.ProcessingTypes.Where(x => x.IsSelected == true).ToList();

            foreach (var objProcessingType in newSiteProcessingTypes)
            {
                SiteProcessingType siteProcessingType = new SiteProcessingType();
                siteProcessingType.Id = Guid.NewGuid();
                siteProcessingType.ProcessingTypeId = objProcessingType.Id;
                siteProcessingType.SiteId = siteId;
                siteProcessingType.CreatedBy = updatedBy;
                siteProcessingType.CreatedDate = DateTime.Now;
                siteProcessingType.UpdatedBy = updatedBy;
                siteProcessingType.UpdatedDate = DateTime.Now;

                base.BatchAdd(siteProcessingType);
            }

            base.Repository.SaveChanges();
        }
    }
}

