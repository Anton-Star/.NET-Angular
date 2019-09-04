using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSectionQuestionAnswerRepository : IRepository<ApplicationSectionQuestionAnswer>
    {
        /// <summary>
        /// Gets an Application Section Question Answer by id
        /// </summary>
        /// <param name="id">Id of the application section question answer</param>
        /// <returns>ApplicationSectionQuestionAnswer object</returns>
        ApplicationSectionQuestionAnswer GetById(Guid id);
        /// <summary>
        /// Gets all Application Section Question Answer by question id
        /// </summary>
        /// <param name="questionId">Id of the application section question</param>
        /// <returns>Collection of ApplicationSectionQuestionAnswer object</returns>
        List<ApplicationSectionQuestionAnswer> GetByQuestion(Guid questionId);
        /// <summary>
        /// Gets all Application Section Question Answer by question id asynchronously
        /// </summary>
        /// <param name="questionId">Id of the application section question</param>
        /// <returns>Collection of ApplicationSectionQuestionAnswer object</returns>
        Task<List<ApplicationSectionQuestionAnswer>> GetByQuestionAsync(Guid questionId);

        List<QuestionAnswer> GetSectionAnswers(Guid applicationSectionId);
        List<ApplicationSectionQuestionAnswer> GetAllForVersion(Guid applicationVersionId);
    }
}
