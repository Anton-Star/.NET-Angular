using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class InspectionScheduleSiteManager : BaseManager<InspectionScheduleSiteManager, IInspectionScheduleSiteRepository, InspectionScheduleSite>
    {
        public InspectionScheduleSiteManager(IInspectionScheduleSiteRepository repository) : base(repository)
        {

        }

        /// <summary>
        /// Get all inpection schedule details against inspection schedule id
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleSite> GetSiteByInspectionScheduleId(int inspectionScheduleId)
        {
            LogMessage("GetSiteByInspectionScheduleId (InspectionScheduleSiteManager)");

            return base.Repository.GetSiteByInspectionScheduleId(inspectionScheduleId);
        }

        /// <summary>
        /// Get all inpection schedule details against inspection schedule id asynchrously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleSite>> GetFacilityByInspectionScheduleIdAsync(int inspectionScheduleId)
        {
            LogMessage("GetSiteByInspectionScheduleIdAsync (InspectionScheduleSiteManager)");

            return base.Repository.GetSiteByInspectionScheduleIdAsync(inspectionScheduleId);
        }
    }
}
