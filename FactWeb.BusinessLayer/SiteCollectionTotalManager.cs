using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteCollectionTotalManager : BaseManager<SiteCollectionTotalManager, ISiteCollectionTotalRepository, SiteCollectionTotal>
    {
        public SiteCollectionTotalManager(ISiteCollectionTotalRepository repository) : base(repository)
        {
        }

        public async Task<SiteCollectionTotal> SaveSiteCollectionTotalAsync(CollectionType collectionType,
            ClinicalPopulationType popType, Guid? id, int siteId, int numberOfUnits, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteCollectionTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.CollectionTypeId = collectionType.Id;
                row.ClinicalPopulationTypeId = popType.Id;
                row.NumberOfUnits = numberOfUnits;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                await base.SaveAsync(row);
            }
            else
            {
                row = new SiteCollectionTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    CollectionTypeId = collectionType.Id,
                    ClinicalPopulationTypeId = popType.Id,
                    NumberOfUnits = numberOfUnits,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                await base.Repository.AddAsync(row);
            }

            return row;
        }

        public SiteCollectionTotal SaveSiteCollectionTotal(CollectionType collectionType, ClinicalPopulationType popType,
            Guid? id, int siteId, int numberOfUnits, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteCollectionTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.CollectionTypeId = collectionType.Id;
                row.ClinicalPopulationTypeId = popType.Id;
                row.NumberOfUnits = numberOfUnits;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                base.Save(row);
            }
            else
            {
                row = new SiteCollectionTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    CollectionTypeId = collectionType.Id,
                    ClinicalPopulationTypeId = popType.Id,
                    NumberOfUnits = numberOfUnits,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                base.Repository.Add(row);
            }

            return row;
        }
    }
}
