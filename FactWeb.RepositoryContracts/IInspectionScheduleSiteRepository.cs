using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IInspectionScheduleSiteRepository : IRepository<InspectionScheduleSite>
    {
        List<InspectionScheduleSite> GetSiteByInspectionScheduleId(int inspectionScheduleId);
        Task<List<InspectionScheduleSite>> GetSiteByInspectionScheduleIdAsync(int inspectionScheduleId);        
    }
}
