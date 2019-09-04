using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteTransplantTotalManager : BaseManager<SiteTransplantTotalManager, ISiteTransplantTotalRepository, SiteTransplantTotal>
    {
        public SiteTransplantTotalManager(ISiteTransplantTotalRepository repository) : base(repository)
        {
        }

        public List<SiteTransplantTotal> GetAllBySite(int siteId)
        {
            return base.Repository.GetAllBySite(siteId);
        }

        public Task<List<SiteTransplantTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.Repository.GetAllBySiteAsync(siteId);
        }

        public async Task<SiteTransplantTotal> SaveSiteTransplantTotalAsync(TransplantCellType cellType,
            ClinicalPopulationType popType, TransplantType transplantType, Guid? id, int siteId, bool isHaploid,
            int numberOfUnits, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteTransplantTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.TransplantCellTypeId = cellType.Id;
                row.ClinicalPopulationTypeId = popType.Id;
                row.TransplantTypeId = transplantType.Id;
                row.IsHaploid = isHaploid;
                row.NumberOfUnits = numberOfUnits;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                await base.SaveAsync(row);
            }
            else
            {
                row = new SiteTransplantTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    TransplantCellTypeId = cellType.Id,
                    ClinicalPopulationTypeId = popType.Id,
                    TransplantTypeId = transplantType.Id,
                    IsHaploid = isHaploid,
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

        public SiteTransplantTotal SaveSiteTransplantTotal(TransplantCellType cellType, ClinicalPopulationType popType,
            TransplantType transplantType, Guid? id, int siteId, bool isHaploid, int numberOfUnits, DateTime startDate, DateTime endDate,
            string savedBy)
        {
            SiteTransplantTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.TransplantCellTypeId = cellType.Id;
                row.ClinicalPopulationTypeId = popType.Id;
                row.TransplantTypeId = transplantType.Id;
                row.IsHaploid = isHaploid;
                row.NumberOfUnits = numberOfUnits;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                base.Save(row);
            }
            else
            {
                row = new SiteTransplantTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    TransplantCellTypeId = cellType.Id,
                    ClinicalPopulationTypeId = popType.Id,
                    TransplantTypeId = transplantType.Id,
                    IsHaploid = isHaploid,
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
