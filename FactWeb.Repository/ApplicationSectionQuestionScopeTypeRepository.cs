using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationSectionQuestionScopeTypeRepository : BaseRepository<ApplicationSectionQuestionScopeType>, IApplicationSectionQuestionScopeTypeRepository
    {
        public ApplicationSectionQuestionScopeTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationSectionQuestionScopeType> GetAllByQuestion(Guid questionId)
        {
            return base.FetchMany(x => x.ApplicationSectionQuestionId == questionId);
        }

        public Task<List<ApplicationSectionQuestionScopeType>> GetAllByQuestionAsync(Guid questionId)
        {
            return base.FetchManyAsync(x => x.ApplicationSectionQuestionId == questionId);
        }
    }
}
