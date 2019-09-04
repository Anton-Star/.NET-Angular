using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationResponseCommentRepository : IRepository<ApplicationResponseComment>
    {

        /// <summary>
        /// Get all application response comments by applicaiton id and question id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        List<ApplicationResponseComment> GetByApplicationIdQuestionId(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId);

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId);

        List<ApplicationResponseComment> GetByApplication(int applicationId);
        List<ApplicationResponseComment> GetByCompliance(Guid complianceId);

            /// <summary>
            /// Get all application response comments by applicaiton id and question id
            /// </summary>
            /// <param name="applicationId"></param>
            /// <param name="questionId"></param>
            /// <returns></returns>
        List<ApplicationResponseComment> GetByApplicationIdQuestionId(int applicationId, Guid questionId);

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId);

        List<ApplicationResponseComment> GetAllByCompliance(Guid complianceApplicationId);

        List<SectionComment> GetSectionComments(Guid applicationUniqueId, Guid applicationSectionId);
    }
}

