using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Linq;

namespace FactWeb.BusinessLayer
{
    public class SiteClinicalTypeManager : BaseManager<SiteClinicalTypeManager, ISiteClinicalTypeRepository, SiteClinicalType>
    {
        public SiteClinicalTypeManager(ISiteClinicalTypeRepository repository) : base(repository)
        {
        }

        public void AddorUpdateSiteClinicalType(SiteItems siteItem, int siteId, string updatedBy)
        {
            LogMessage("AddorUpdateSiteClinicalType (SiteClinicalTypeManager)");

            var existingSiteClinicalTypes = base.Repository.GetAllBySiteId(siteId);

            foreach (var item in existingSiteClinicalTypes)
            {
                base.BatchRemove(item);
            }

            var newSiteClinicalTypes = siteItem.ClinicalTypes.Where(x => x.IsSelected == true).ToList();

            foreach (var objClinicalType in newSiteClinicalTypes)
            {
                SiteClinicalType siteClinicalType = new SiteClinicalType();
                siteClinicalType.Id = Guid.NewGuid();
                siteClinicalType.ClinicalTypeId = objClinicalType.Id;
                siteClinicalType.SiteId = siteId;
                siteClinicalType.CreatedBy = updatedBy;
                siteClinicalType.CreatedDate = DateTime.Now;
                siteClinicalType.UpdatedBy = updatedBy;
                siteClinicalType.UpdatedDate = DateTime.Now;

                base.BatchAdd(siteClinicalType);
            }

            base.Repository.SaveChanges();
        }
    }
}

