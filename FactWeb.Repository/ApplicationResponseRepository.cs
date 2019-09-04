using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationResponseRepository : BaseRepository<ApplicationResponse>, IApplicationResponseRepository
    {
        public ApplicationResponseRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationResponse> GetApplicationResponses(long organizationId, int applicationTypeId)
        {
            return base.Context.ApplicationResponses.Where(x =>
                x.Application.OrganizationId == organizationId &&
                x.Application.ApplicationTypeId == applicationTypeId)
                .Include(x => x.ApplicationResponseStatus)
                .ToList();

        }

        public Task<List<ApplicationResponse>> GetApplicationResponsesAsync(long organizationId, int applicationTypeId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.Application.OrganizationId == organizationId &&
                        x.Application.ApplicationTypeId == applicationTypeId);
        }

        //public Task<List<ApplicationResponse>> GetApplicationResponsesWithDocumentsAsync(int organizationId, Guid documentId)
        //{
        //    return base.FetchManyAsync(x => x.Application.OrganizationId == organizationId && x.DocumentId == documentId);
        //}

        //public List<ApplicationResponse> GetApplicationResponsesWithDocuments(int organizationId, Guid documentId)
        //{
        //    return base.FetchMany(x => x.Application.OrganizationId == organizationId && x.DocumentId == documentId);
        //}

        public Task<List<ApplicationResponse>> GetApplicationResponsesWithDocumentsAsync(int organizationId, Guid documentId)
        {
            return base.FetchManyAsync(x => x.Application.OrganizationId == organizationId && x.DocumentId == documentId);
        }

        public List<ApplicationResponse> GetApplicationResponsesWithDocuments(int organizationId, Guid documentId)
        {
            return base.FetchMany(x => x.Application.OrganizationId == organizationId && x.DocumentId == documentId);
        }

        public List<ApplicationResponse> GetApplicationResponses(Guid applicationUniqueId)
        {
            //return base.FetchMany(x => x.Application.UniqueId == applicationUniqueId);

            //return base.Context.ApplicationResponses.Where(x => x.Application.UniqueId == applicationUniqueId).Include(y => y.Documents).ToList();

            return base.Context.ApplicationResponses.Where(x => x.Application.UniqueId == applicationUniqueId)
                .Include(x => x.ApplicationResponseStatus)
                .Include(x => x.VisibleApplicationResponseStatus)
                .Include(x=>x.ApplicationSectionQuestion)
                .Include(x=>x.Document)
                .ToList();


            //.Include(x => x.ApplicationResponseStatus)
        }

        public List<ApplicationResponse> GetApplicationResponses(int organizationId, int applicationResponseStatusId)
        {
            return
                base.FetchMany(
                    x =>
                        x.Application.OrganizationId == organizationId &&
                        x.ApplicationResponseStatusId == applicationResponseStatusId);
        }

        public List<ApplicationResponse> GetResponsesByAppIdQuestionId(Guid? questionId, int applicationId)
        {
            return base.FetchMany(x => x.ApplicationId == applicationId && x.ApplicationSectionQuestionId == questionId);
        }

        public List<ApplicationResponse> GetApplicationRfis(int organizationId)
        {
            return
                base.FetchMany(
                    x =>
                        x.Application.OrganizationId == organizationId &&
                        (x.ApplicationResponseStatus.Name == Constants.ApplicationResponseStatus.RFI || x.ApplicationResponseStatus.Name == Constants.ApplicationResponseStatus.RFICompleted));
        }

        public List<ApplicationResponse> GetAllForCompliance(Guid complianceId)
        {
            return base.Context.ApplicationResponses
                .Include(x => x.ApplicationResponseStatus)
                .Include(x => x.VisibleApplicationResponseStatus)
                .Include(x => x.ApplicationSectionQuestion)
                .Include(x=>x.ApplicationSectionQuestion.ApplicationSection)
                .Include(x => x.Document)
                .Where(x => x.Application.ComplianceApplicationId == complianceId)
                .ToList();
        }

        public void AddToHistory(Guid applicationUniqueId, Guid sectionId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[2];

            paramList[0] = applicationUniqueId;
            paramList[1] = sectionId;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_addResponseHistory @ApplicationUniqueId={0}, @SectionId={1}", paramList);
        }

        public void BulkUpdate(int applicationId, Guid applicationSectionId, int fromStatusId, int toStatusId, string updatedBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[5];

            paramList[0] = applicationId;
            paramList[1] = applicationSectionId;
            paramList[2] = fromStatusId;
            paramList[3] = toStatusId;
            paramList[4] = updatedBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_BulkUpdate @ApplicationId={0}, @ApplicationSectionId={1}, @FromStatusId={2}, @ToStatusId={3}, @UpdatedBy={4}", paramList);
        }

        public void RemoveForSection(Guid applicationUniqueId, Guid sectionId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[2];

            paramList[0] = applicationUniqueId;
            paramList[1] = sectionId;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_deleteSectionResponses @ApplicationUniqueId={0}, @SectionId={1}", paramList);
        }

        public void ProcessExpectedAndHidden(Guid applicationUniqueId, string updatedBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[2];

            paramList[0] = applicationUniqueId;
            paramList[1] = updatedBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_processExpectedAndHiddens @ApplicationUniqueId={0}, @UpdatedBy={1}", paramList);
        }
    }
}
