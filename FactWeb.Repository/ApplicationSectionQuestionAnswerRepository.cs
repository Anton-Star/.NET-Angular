using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationSectionQuestionAnswerRepository : BaseRepository<ApplicationSectionQuestionAnswer>, IApplicationSectionQuestionAnswerRepository
    {
        public ApplicationSectionQuestionAnswerRepository(FactWebContext context) : base(context)
        {
        }

        public override ApplicationSectionQuestionAnswer GetById(int id)
        {
            throw new NotImplementedException("Use GetById Guid");
        }

        public ApplicationSectionQuestionAnswer GetById(Guid id)
        {
            return base.Dbset.Find(id);
        }

        public List<ApplicationSectionQuestionAnswer> GetByQuestion(Guid questionId)
        {
            return base.FetchMany(x => x.ApplicationSectionQuestionId == questionId && x.IsActive);
        }

        public Task<List<ApplicationSectionQuestionAnswer>> GetByQuestionAsync(Guid questionId)
        {
            return base.FetchManyAsync(x => x.ApplicationSectionQuestionId == questionId && x.IsActive);
        }

        public List<QuestionAnswer> GetSectionAnswers(Guid applicationSectionId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = applicationSectionId;

            var rows = objectContext.ExecuteStoreQuery<QuestionAnswer>(
                "EXEC usp_getApplicationSectionQuestionAnswers @ApplicationSectionId={0}", paramList).ToList();

            return rows;
        }

        public List<ApplicationSectionQuestionAnswer> GetAllForVersion(Guid applicationVersionId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = applicationVersionId;

            var rows = objectContext.ExecuteStoreQuery<ApplicationSectionQuestionAnswer>(
                "EXEC usp_getAnswersByVersion @ApplicationVersionId={0}", paramList).ToList();

            return rows;
        }
    }
}
