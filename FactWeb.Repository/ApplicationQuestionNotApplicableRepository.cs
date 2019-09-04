using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationQuestionNotApplicableRepository : BaseRepository<ApplicationQuestionNotApplicable>, IApplicationQuestionNotApplicableRepository
    {
        public ApplicationQuestionNotApplicableRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationQuestionNotApplicable> GetByOrganization(int organizationId)
        {
            return base.FetchMany(x => x.Application.ComplianceApplication.OrganizationId == organizationId &&
                                            x.Application.ComplianceApplication.IsActive);
        }

        public Task<List<ApplicationQuestionNotApplicable>> GetByOrganizationAsync(int organizationId)
        {
            return base.FetchManyAsync(x => x.Application.ComplianceApplication.OrganizationId == organizationId &&
                                            x.Application.ComplianceApplication.IsActive);
        }

        public List<ApplicationQuestionNotApplicable> GetByApplication(int applicationId)
        {
            return base.FetchMany(x => x.ApplicationId == applicationId);
        }

        public List<ApplicationQuestionNotApplicable> GetAllForActiveVersionNoLazyLoad()
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationQuestionNotApplicables
                .AsNoTracking()
                .Where(
                    x =>
                        x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersion.IsActive &&
                        x.ApplicationSectionQuestion.IsActive &&
                        (x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersion.IsDeleted == null ||
                         x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersion.IsDeleted == false))
                .ToList();
        }

        public List<ApplicationQuestionNotApplicable> GetAllForVersionNoLazyLoad(Guid versionId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationQuestionNotApplicables
                .AsNoTracking()
                .Where(
                    x =>
                        x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersionId == versionId &&
                        x.ApplicationSectionQuestion.IsActive)
                .ToList();
        }

        public List<ApplicationQuestionNotApplicable> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationQuestionNotApplicables
                .AsNoTracking()
                .Where(
                    x =>
                        x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersion.ApplicationType.Name ==
                        applicationTypeName &&
                        x.ApplicationSectionQuestion.IsActive &&
                        (x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersion.IsDeleted == null ||
                         x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersion.IsDeleted == false))
                .ToList();
        }

        public List<ApplicationQuestionNotApplicable> GetAllForCompliance(Guid complianceApplicationId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return
                base.Context.ApplicationQuestionNotApplicables.Where(
                    x => x.Application.ComplianceApplicationId == complianceApplicationId).ToList();
        }

        public void RemoveForCompliance(Guid complianceApplicationId, string removedBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[2];

            paramList[0] = complianceApplicationId;
            paramList[1] = removedBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_removeNotApplicables @ComplianceApplicationId={0}, @RemovedBy={1}", paramList);
        }

        public void AddForApplication(int applicationId, string questionIds, string createdBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[3];

            paramList[0] = applicationId;
            paramList[1] = questionIds;
            paramList[2] = createdBy;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_InsertNotApplicables @ApplicationId={0}, @QuestionIds={1}, @CreatedBy={2}", paramList);
        }
    }
}
