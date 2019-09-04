using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class InspectionRepository : BaseRepository<Inspection>, IInspectionRepository
    {
        public InspectionRepository(FactWebContext context) : base(context)
        {
        }

        public Task<Inspection> GetInspectionByAppIdAsync(Guid applicationUniqueId)
        { 
            return
                base.FetchAsync(
                    x =>
                        x.Application.UniqueId == applicationUniqueId);
        }

        public Inspection GetByApplication(Guid applicationUniqueId, bool isTrainee, bool isReinspection)
        {
            return
                base.Fetch(
                    x =>
                        x.Application.UniqueId == applicationUniqueId &&
                        ((x.IsTrainee.HasValue && x.IsTrainee.Value == isTrainee) ||
                         (!x.IsTrainee.HasValue && !isTrainee)) &&
                        ((x.IsReinspection.HasValue && x.IsReinspection.Value == isReinspection) ||
                         (!x.IsReinspection.HasValue && !isReinspection)));
        }

        public Task<Inspection> GetByApplicationAsync(Guid applicationUniqueId, bool isTrainee, bool isReinspection)
        {
            return
                base.FetchAsync(
                    x =>
                        x.Application.UniqueId == applicationUniqueId &&
                        ((x.IsTrainee.HasValue && x.IsTrainee.Value == isTrainee) ||
                         (!x.IsTrainee.HasValue && !isTrainee)) &&
                        ((x.IsReinspection.HasValue && x.IsReinspection.Value == isReinspection) ||
                         (!x.IsReinspection.HasValue && !isReinspection)));
        }

        public Inspection GetByApplicationAndSite(Guid complianceAppId, string siteName, bool isTrainee, bool isReinspection)
        {
            return
                base.Fetch(
                    x =>
                        x.Application.ComplianceApplicationId.Value == complianceAppId &&
                        x.Application.Site.Name == siteName &&
                        ((x.IsTrainee.HasValue && x.IsTrainee.Value == isTrainee) ||
                         (!x.IsTrainee.HasValue && !isTrainee)) &&
                        ((x.IsReinspection.HasValue && x.IsReinspection.Value == isReinspection) ||
                         (!x.IsReinspection.HasValue && !isReinspection)));
        }

        public Task<Inspection> GetByApplicationAndSiteAsync(Guid complianceAppId, string siteName, bool isTrainee, bool isReinspection)
        {
            return
                base.FetchAsync(
                    x =>
                        x.Application.ComplianceApplicationId.Value == complianceAppId &&
                        x.Application.Site.Name == siteName &&
                        ((x.IsTrainee.HasValue && x.IsTrainee.Value == isTrainee) ||
                         (!x.IsTrainee.HasValue && !isTrainee)) &&
                        ((x.IsReinspection.HasValue && x.IsReinspection.Value == isReinspection) ||
                         (!x.IsReinspection.HasValue && !isReinspection)));
        }

        public List<InspectionDetail> GetInspectionDetails(Guid complianceAppId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = complianceAppId;


            return objectContext.ExecuteStoreQuery<InspectionDetail>(
                "EXEC usp_getInspectionDetails @complianceApplicationId={0}",
                paramList).ToList();
        }

        public InspectionOverallDetail GetInspectionOverallDetails(Guid complianceAppId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = complianceAppId;


            return objectContext.ExecuteStoreQuery<InspectionOverallDetail>(
                "EXEC usp_getInspectionOverallDetails @complianceApplicationId={0}",
                paramList).FirstOrDefault();
        }
    }
}

