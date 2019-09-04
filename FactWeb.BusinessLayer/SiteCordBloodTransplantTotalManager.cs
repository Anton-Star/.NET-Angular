using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteCordBloodTransplantTotalManager : BaseManager<SiteCordBloodTransplantTotalManager, ISiteCordBloodTransplantTotalRepository, SiteCordBloodTransplantTotal>
    {
        public SiteCordBloodTransplantTotalManager(ISiteCordBloodTransplantTotalRepository repository) : base(repository)
        {
        }

        public List<SiteCordBloodTransplantTotal> GetBySite(int siteId)
        {
            return base.Repository.GetBySite(siteId);
        }

        public Task<List<SiteCordBloodTransplantTotal>> GetBySiteAsync(int siteId)
        {
            return base.Repository.GetBySiteAsync(siteId);
        }

        public SiteCordBloodTransplantTotal SaveSiteCordBloodTransplantTotal(CBUnitType unitType, CBCategory category, Guid? id, int siteId, int numberOfUnits, DateTime asOfDate, string savedBy)
        {
            SiteCordBloodTransplantTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.CBCategoryId = category.Id;
                row.CBUnitTypeId = unitType.Id;
                row.NumberOfUnits = numberOfUnits;
                row.AsOfDate = asOfDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                base.Save(row);
            }
            else
            {
                row = new SiteCordBloodTransplantTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    CBCategoryId = category.Id,
                    CBUnitTypeId = unitType.Id,
                    NumberOfUnits = numberOfUnits,
                    AsOfDate = asOfDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                base.Add(row);
            }

            return row;
        }

        public async Task<SiteCordBloodTransplantTotal> SaveSiteCordBloodTransplantTotalAsync(CBUnitType unitType, CBCategory category, Guid? id, int siteId, int numberOfUnits, DateTime asOfDate, string savedBy)
        {
            SiteCordBloodTransplantTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.CBCategoryId = category.Id;
                row.CBUnitTypeId = unitType.Id;
                row.NumberOfUnits = numberOfUnits;
                row.AsOfDate = asOfDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                await base.SaveAsync(row);
            }
            else
            {
                row = new SiteCordBloodTransplantTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    CBCategoryId = category.Id,
                    CBUnitTypeId = unitType.Id,
                    NumberOfUnits = numberOfUnits,
                    AsOfDate = asOfDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                await base.Repository.AddAsync(row);
            }

            return row;
        }
    }
}
