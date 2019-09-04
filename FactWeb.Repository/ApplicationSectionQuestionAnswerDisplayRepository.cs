using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationSectionQuestionAnswerDisplayRepository : BaseRepository<ApplicationSectionQuestionAnswerDisplay>, IApplicationSectionQuestionAnswerDisplayRepository
    {
        public ApplicationSectionQuestionAnswerDisplayRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllForApplicationType(int applicationTypeId)
        {
            return
                base.FetchMany(
                    x =>
                        x.ApplicationSectionQuestionAnswer.Question.ApplicationSection
                            .ApplicationTypeId == applicationTypeId);
        }

        public Task<List<ApplicationSectionQuestionAnswerDisplay>> GetAllForApplicationTypeAsync(int applicationTypeId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.ApplicationSectionQuestionAnswer.Question.ApplicationSection
                            .ApplicationTypeId == applicationTypeId);
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllForQuestion(Guid questionId)
        {
            return base.Context.ApplicationSectionQuestionAnswerDisplays
                .Include(x=>x.ApplicationSectionQuestion)
                .Where(x => x.ApplicationSectionQuestionAnswer.ApplicationSectionQuestionId == questionId)
                .ToList();
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllForVersion(Guid versionId)
        {
            return base.FetchMany(x => x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersionId == versionId);
        }

        public List<QuestionAnswerDisplay> GetDisplaysForSection(Guid applicationSectionId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = applicationSectionId;

            var rows = objectContext.ExecuteStoreQuery<QuestionAnswerDisplay>(
                "EXEC usp_getApplicationSectionQuestionAnswerDisplays @ApplicationSectionId={0}", paramList).ToList();

            return rows;
        }
    }
}
