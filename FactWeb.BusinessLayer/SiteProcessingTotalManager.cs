using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class SiteProcessingTotalManager : BaseManager<SiteProcessingTotalManager, ISiteProcessingTotalRepository, SiteProcessingTotal>
    {
        public SiteProcessingTotalManager(ISiteProcessingTotalRepository repository) : base(repository)
        {
        }

        public List<SiteProcessingTotal> GetAllBySite(int siteId)
        {
            return base.Repository.GetAllBySite(siteId);
        }

        public Task<List<SiteProcessingTotal>> GetAllBySiteAsync(int siteId)
        {
            return base.Repository.GetAllBySiteAsync(siteId);
        }

        public async Task<SiteProcessingTotal> SaveAsync(List<TransplantCellType> cellTypes, Guid? id, int siteId, int numberOfUnits, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteProcessingTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }
                
                row.NumberOfUnits = numberOfUnits;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                await base.SaveAsync(row);
            }
            else
            {
                if (cellTypes == null || cellTypes.Count  == 0)
                {
                    throw new Exception("You must select Transplant Cell Types.");
                }

                row = new SiteProcessingTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    NumberOfUnits = numberOfUnits,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy,
                    SiteProcessingTotalTransplantCellTypes =
                        cellTypes.Select(x => new SiteProcessingTotalTransplantCellType
                        {
                            Id = Guid.NewGuid(),
                            TransplantCellTypeId = x.Id,
                            CreatedDate = DateTime.Now,
                            CreatedBy = savedBy
                        })
                            .ToList()
                };

                

                await base.Repository.AddAsync(row);
            }

            return row;
        }

        public SiteProcessingTotal Save(List<TransplantCellType> cellTypes, Guid? id, int siteId, int numberOfUnits, DateTime startDate, DateTime endDate, string savedBy)
        {
            SiteProcessingTotal row = null;
            if (id.HasValue)
            {
                row = this.GetById(id.Value);

                if (row == null || row.SiteId != siteId)
                {
                    throw new Exception("Cannot find total");
                }

                row.NumberOfUnits = numberOfUnits;
                row.StartDate = startDate;
                row.EndDate = endDate;
                row.UpdatedBy = savedBy;
                row.UpdatedDate = DateTime.Now;

                base.Save(row);
            }
            else
            {
                if (cellTypes == null || cellTypes.Count == 0)
                {
                    throw new Exception("You must select Transplant Cell Types.");
                }

                row = new SiteProcessingTotal
                {
                    Id = Guid.NewGuid(),
                    SiteId = siteId,
                    NumberOfUnits = numberOfUnits,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = savedBy,
                    SiteProcessingTotalTransplantCellTypes =
                        cellTypes.Select(x => new SiteProcessingTotalTransplantCellType
                        {
                            Id = Guid.NewGuid(),
                            TransplantCellTypeId = x.Id,
                            CreatedDate = DateTime.Now,
                            CreatedBy = savedBy
                        })
                            .ToList()
                };

                base.Repository.Add(row);
            }

            return row;
        }
    }
}
