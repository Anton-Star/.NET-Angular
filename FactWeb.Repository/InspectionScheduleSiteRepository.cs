using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class InspectionScheduleSiteRepository : BaseRepository<InspectionScheduleSite>, IInspectionScheduleSiteRepository
    {
        public InspectionScheduleSiteRepository(FactWebContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all sites selected in an inspection schedule
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleSite> GetSiteByInspectionScheduleId(int inspectionScheduleId)
        {
            return base.FetchMany(x => x.InspectionScheduleId == inspectionScheduleId);
        }

        /// <summary>
        /// Get all sites selected in an inspection schedule asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleSite>> GetSiteByInspectionScheduleIdAsync(int inspectionScheduleId)
        {
            return base.FetchManyAsync(x => x.InspectionScheduleId == inspectionScheduleId);
        }
    }
}
