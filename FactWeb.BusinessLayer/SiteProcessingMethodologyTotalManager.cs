using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteProcessingMethodologyTotalManager : BaseManager<SiteProcessingMethodologyTotalManager, ISiteProcessingMethodologyTotalRepository, SiteProcessingMethodologyTotal>
    {
        public SiteProcessingMethodologyTotalManager(ISiteProcessingMethodologyTotalRepository repository) : base(repository)
        {
        }

        public List<SiteProcessingMethodologyTotal> GetAllBySite(int siteId)
        {
            return base.Repository.GetAllBySite(siteId);
        }

        public Task<List<SiteProcessingMethodologyTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.Repository.GetAllBySiteAsync(siteId);
        }

        public async Task<SiteProcessingMethodologyTotal> SaveAsync(ProcessingType processingType, Guid? id, int siteId,
            int platformCount, int protocolCount, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteProcessingMethodologyTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.ProcessingTypeId = processingType.Id;
                row.PlatformCount = platformCount;
                row.ProtocolCount = protocolCount;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                await base.SaveAsync(row);
            }
            else
            {
                row = new SiteProcessingMethodologyTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    ProcessingTypeId = processingType.Id,
                    ProtocolCount = protocolCount,
                    PlatformCount = platformCount,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy
                };

                await base.Repository.AddAsync(row);
            }

            return row;
        }

        public SiteProcessingMethodologyTotal Save(ProcessingType processingType, Guid? id, int siteId, int platformCount, int protocolCount, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteProcessingMethodologyTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.ProcessingTypeId = processingType.Id;
                row.PlatformCount = platformCount;
                row.ProtocolCount = protocolCount;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                base.Save(row);
            }
            else
            {
                row = new SiteProcessingMethodologyTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    ProcessingTypeId = processingType.Id,
                    ProtocolCount = protocolCount,
                    PlatformCount = platformCount,
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
