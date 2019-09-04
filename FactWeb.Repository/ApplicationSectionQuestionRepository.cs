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
    public class ApplicationSectionQuestionRepository : BaseRepository<ApplicationSectionQuestion>, IApplicationSectionQuestionRepository
    {
        public ApplicationSectionQuestionRepository(FactWebContext context) : base(context)
        {
        }

        public override ApplicationSectionQuestion GetById(int id)
        {
            throw new NotImplementedException("Use GetById Guid");
        }

        public ApplicationSectionQuestion GetById(Guid id)
        {
            return base.Dbset.Find(id);
        }

        public List<ApplicationSectionQuestion> GetAllByApplicationType(int applicationTypeId)
        {
            return base.FetchMany(x => x.ApplicationSection.ApplicationTypeId == applicationTypeId);
        }

        public Task<List<ApplicationSectionQuestion>> GetAllByApplicationTypeAsync(int applicationTypeId)
        {
            return base.FetchManyAsync(x => x.ApplicationSection.ApplicationTypeId == applicationTypeId);
        }

        public List<ApplicationSectionQuestion> GetAllForActiveVersionsNoLazyLoad()
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationSectionQuestions
                .AsNoTracking()
                .Include(x => x.QuestionType)
                .Include(x => x.Answers)
                .Include(x => x.ScopeTypes)
                .Include(x => x.ScopeTypes.Select(y => y.ScopeType))
                .Where(
                    x =>
                        x.IsActive && x.ApplicationSection.ApplicationVersion.IsActive &&
                        (x.ApplicationSection.ApplicationVersion.IsDeleted == null ||
                         x.ApplicationSection.ApplicationVersion.IsDeleted == false))
                .ToList();
        }

        public List<ApplicationSectionQuestion> GetAllForVersionNoLazyLoad(Guid versionId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationSectionQuestions
                .AsNoTracking()
                .Include(x => x.QuestionType)
                .Include(x => x.Answers)
                .Include(x => x.ScopeTypes)
                .Include(x => x.ScopeTypes.Select(y => y.ScopeType))
                .Where(
                    x =>
                        x.IsActive && x.ApplicationSection.ApplicationVersionId == versionId)
                .ToList();
        }

        public List<ApplicationSectionQuestion> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationSectionQuestions
                .Include(x => x.QuestionType)
                .Include(x => x.Answers)
                .Include(x => x.Answers.Select(y => y.Displays))
                .Include(x => x.ScopeTypes)
                .Include(x => x.ScopeTypes.Select(y => y.ScopeType))
                .Include(x => x.HiddenBy)
                .Where(
                    x =>
                        x.ApplicationSection.ApplicationType.Name == applicationTypeName && (x.ApplicationSection.ApplicationVersion.IsDeleted == null || x.ApplicationSection.ApplicationVersion.IsDeleted == false))
                .ToList();
        }

        public List<SectionQuestion> GetSectionQuestions(Guid applicationUniqueId, Guid applicationSectionId, Guid userId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[3];
            paramList[0] = applicationUniqueId;
            paramList[1] = applicationSectionId;
            paramList[2] = userId;

            var rows = objectContext.ExecuteStoreQuery<SectionQuestion>(
                "EXEC usp_getApplicationSectionQuestions @ApplicationUniqueId={0}, @ApplicationSectionId={1}, @UserId={2}", paramList).ToList();

            return rows;
        }

        public List<ApplicationSectionQuestion> GetQuestionsWithRFIs(int applicationId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = applicationId;

            var rows = objectContext.ExecuteStoreQuery<ApplicationSectionQuestion>(
                "EXEC usp_getQuestionsWithRFI @ApplicationId={0}", paramList).ToList();

            return rows;
        }
    }
}
