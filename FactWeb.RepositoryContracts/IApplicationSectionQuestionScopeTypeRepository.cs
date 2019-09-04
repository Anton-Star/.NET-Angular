using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSectionQuestionScopeTypeRepository : IRepository<ApplicationSectionQuestionScopeType>
    {
        /// <summary>
        /// Gets all the scopes for a specific question
        /// </summary>
        /// <param name="questionId">Id of the question</param>
        /// <returns>Collection of ApplicationScheduleQuestionScopeType objects</returns>
        List<ApplicationSectionQuestionScopeType> GetAllByQuestion(Guid questionId);
        /// <summary>
        /// Gets all the scopes for a specific question asynchronously
        /// </summary>
        /// <param name="questionId">Id of the question</param>
        /// <returns>Collection of ApplicationScheduleQuestionScopeType objects</returns>
        Task<List<ApplicationSectionQuestionScopeType>> GetAllByQuestionAsync(Guid questionId);
    }
}
