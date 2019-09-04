using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSectionQuestionScopeTypeManager : BaseManager<ApplicationSectionQuestionScopeTypeManager, IApplicationSectionQuestionScopeTypeRepository, ApplicationSectionQuestionScopeType>
    {
        public ApplicationSectionQuestionScopeTypeManager(IApplicationSectionQuestionScopeTypeRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets all the scopes for a specific question
        /// </summary>
        /// <param name="questionId">Id of the question</param>
        /// <returns>Collection of ApplicationScheduleQuestionScopeType objects</returns>
        public List<ApplicationSectionQuestionScopeType> GetAllByQuestion(Guid questionId)
        {
            LogMessage("GetAllByQuestion (ApplicationSectionQuestionScopeTypeManager)");

            return base.Repository.GetAllByQuestion(questionId);
        }

        /// <summary>
        /// Gets all the scopes for a specific question asynchronously
        /// </summary>
        /// <param name="questionId">Id of the question</param>
        /// <returns>Collection of ApplicationScheduleQuestionScopeType objects</returns>
        public Task<List<ApplicationSectionQuestionScopeType>> GetAllByQuestionAsync(Guid questionId)
        {
            LogMessage("GetAllByQuestionAsync (ApplicationSectionQuestionScopeTypeManager)");

            return base.Repository.GetAllByQuestionAsync(questionId);
        }

        public void RemoveItems(Guid applicationSectionQuestionId)
        {
            LogMessage("RemoveItems (ApplicationSectionQuestionScopeTypeManager)");

            var items = this.GetAllByQuestion(applicationSectionQuestionId);

            foreach (var item in items)
            {
                base.BatchRemove(item);
            }
            base.SaveChanges();
        }

        public async Task RemoveItemsAsync(Guid applicationSectionQuestionId)
        {
            LogMessage("RemoveItemsAsync (ApplicationSectionQuestionScopeTypeManager)");

            var items = this.GetAllByQuestion(applicationSectionQuestionId);

            foreach (var item in items)
            {
                base.BatchRemove(item);
            }
            await base.SaveChangesAsync();
        }
    }
}
