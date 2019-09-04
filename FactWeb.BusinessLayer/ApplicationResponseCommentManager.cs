using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{

    public class ApplicationResponseCommentManager : BaseManager<ApplicationResponseCommentManager, IApplicationResponseCommentRepository, ApplicationResponseComment>
    {
        public ApplicationResponseCommentManager(IApplicationResponseCommentRepository repository) : base(repository)
        {

        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public List<ApplicationResponseComment> GetByApplicationIdQuestionId(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId)
        {
            LogMessage("GetByApplicationIdQuestionId (ApplicationResponseCommentManager)");

            return base.Repository.GetByApplicationIdQuestionId(applicationId, questionId, answerResponseStatusId, commentTypeId);
        }

        public List<ApplicationResponseComment> GetByApplication(int applicationId)
        {
            return base.Repository.GetByApplication(applicationId);
        }

        public List<ApplicationResponseComment> GetByCompliance(Guid complianceId)
        {
            return base.Repository.GetByCompliance(complianceId);
        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public async Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId)
        {
            LogMessage("GetByApplicationIdQuestionId (ApplicationResponseCommentManager)");

            return await base.Repository.GetByApplicationIdQuestionIdAsync(applicationId, questionId, answerResponseStatusId, commentTypeId);
        }


        /// <summary>
        /// Get all application response comments by applicaiton id and question id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public List<ApplicationResponseComment> GetByApplicationIdQuestionId(int applicationId, Guid questionId)
        {
            LogMessage("GetByApplicationIdQuestionId (ApplicationResponseCommentManager)");

            return base.Repository.GetByApplicationIdQuestionId(applicationId, questionId);
        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public async Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId)
        {
            LogMessage("GetByApplicationIdQuestionId (ApplicationResponseCommentManager)");

            return await base.Repository.GetByApplicationIdQuestionIdAsync(applicationId, questionId);
        }

        public List<ApplicationResponseComment> GetAllByCompliance(Guid complianceApplicationId)
        {
            return base.Repository.GetAllByCompliance(complianceApplicationId);
        }

        public List<SectionComment> GetSectionComments(Guid applicationUniqueId, Guid applicationSectionId)
        {
            return base.Repository.GetSectionComments(applicationUniqueId, applicationSectionId);
        }
    }
}


