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
    public class ApplicationResponseCommentRepository : BaseRepository<ApplicationResponseComment>, IApplicationResponseCommentRepository
    {
        public ApplicationResponseCommentRepository(FactWebContext context) : base(context)
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
            return base.FetchMany(x => x.ApplicationId == applicationId && x.QuestionId == questionId && x.CommentTypeId == commentTypeId).OrderByDescending(y => y.UpdatedDate).ToList();         
        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId)
        {
              return Task.FromResult(base.FetchManyAsync(x => x.ApplicationId == applicationId && x.QuestionId == questionId && x.CommentTypeId == commentTypeId).Result.OrderByDescending(y => y.UpdatedDate).ToList());

        }


        /// <summary>
        /// Get all application response comments by applicaiton id and question id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public List<ApplicationResponseComment> GetByApplicationIdQuestionId(int applicationId, Guid questionId)
        {
            return base.FetchMany(x => x.ApplicationId == applicationId && x.QuestionId == questionId).OrderByDescending(y => y.UpdatedDate).ToList();
        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId)
        {
            return Task.FromResult(base.FetchManyAsync(x => x.ApplicationId == applicationId && x.QuestionId == questionId).Result.OrderByDescending(y => y.UpdatedDate).ToList());
        }

        public List<ApplicationResponseComment> GetByApplication(int applicationId)
        {
            return base.Context.ApplicationResponseComments
                .Include(x => x.Document)
                .Include(x => x.ApplicationResponseCommentDocuments)
                .Include(x => x.ApplicationResponseCommentDocuments.Select(y => y.Document))
                .Include(x => x.CommentFrom)
                .Include(x => x.CommentFrom.Role)
                .Include(x => x.CommentTo)
                .Include(x => x.CommentTo.Role)
                .Include(x => x.CommentType)
                .Where(x => x.ApplicationId == applicationId)
                .OrderByDescending(y => y.UpdatedDate)
                .ThenByDescending(x => x.CreatedDate)
                .ToList();
        }

        public List<ApplicationResponseComment> GetByCompliance(Guid complianceId)
        {
            return base.Context.ApplicationResponseComments
                .Include(x => x.Document)
                .Include(x => x.CommentFrom)
                .Include(x => x.CommentFrom.Role)
                .Include(x => x.CommentTo)
                .Include(x => x.CommentTo.Role)
                .Include(x => x.CommentType)
                .Where(x => x.Application.ComplianceApplicationId == complianceId)
                .OrderByDescending(y => y.UpdatedDate)
                .ThenByDescending(x => x.CreatedDate)
                .ToList();
        }

        public List<ApplicationResponseComment> GetAllByCompliance(Guid complianceApplicationId)
        {
            return base.Context.ApplicationResponseComments
                .Include(x => x.Document)
                .Include(x => x.CommentFrom)
                .Include(x => x.CommentTo)
                .Include(x => x.CommentType)
                .Where(x => x.Application.ComplianceApplicationId == complianceApplicationId)
                .OrderByDescending(y => y.UpdatedDate)
                .ThenByDescending(x => x.CreatedDate)
                .ToList();
        }

        public List<SectionComment> GetSectionComments(Guid applicationUniqueId, Guid applicationSectionId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[2];
            paramList[0] = applicationUniqueId;
            paramList[1] = applicationSectionId;

            var rows = objectContext.ExecuteStoreQuery<SectionComment>(
                "EXEC usp_getApplicationSectionComments @ApplicationUniqueId={0}, @ApplicationSectionId={1}", paramList).ToList();

            return rows;
        }
    }
}

