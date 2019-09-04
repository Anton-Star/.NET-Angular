using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class ApplicationResponseCommentFacade
    {
        private readonly Container container;

        public ApplicationResponseCommentFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public List<ApplicationResponseComment> GetByApplicationIdQuestionId(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            return applicationResponseCommentManager.GetByApplicationIdQuestionId(applicationId, questionId, answerResponseStatusId, commentTypeId);
        }

        /// <summary>
        /// Get all application response comments by applicaiton id and question id asynchronously
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Task<List<ApplicationResponseComment>> GetByApplicationIdQuestionIdAsync(int applicationId, Guid questionId, int answerResponseStatusId, int commentTypeId)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            return applicationResponseCommentManager.GetByApplicationIdQuestionIdAsync(applicationId, questionId, answerResponseStatusId, commentTypeId);
        }

        public ApplicationResponseComment Save(ApplicationResponseCommentItem applicationResponseCommentItem, Guid currentUserId, string createdBy)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();            
            var associationTypeManager = this.container.GetInstance<AssociationTypeManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var currentUserByEmail = userManager.GetByEmailAddress(createdBy);
            var currentUser = currentUserByEmail;
            
            if (applicationResponseCommentItem.CommentType.Id > 0 && currentUserByEmail.Id != currentUserId)
            {
                currentUser = userManager.GetById(currentUserId);

                var commentType = commentTypeManager.GetById(applicationResponseCommentItem.CommentType.Id);

                if (commentType == null ||
                    (commentType.Name == Constants.CommentTypes.FACTOnly &&
                     currentUserByEmail.Role.Name != Constants.Roles.FACTAdministrator &&
                     currentUserByEmail.Role.Name != Constants.Roles.FACTCoordinator &&
                     currentUserByEmail.Role.Name != Constants.Roles.QualityManager))
                {
                    throw new Exception("Cannot save FACT Only comment");
                }
            }



            ApplicationResponseComment applicationResponseComment = null;

            if (applicationResponseCommentItem.ApplicationResponseCommentId == 0)// new case
            {
                applicationResponseComment = new ApplicationResponseComment
                {
                    QuestionId = applicationResponseCommentItem.QuestionId,
                    ApplicationId = applicationResponseCommentItem.ApplicationId,
                    DocumentId = applicationResponseCommentItem.DocumentId,
                    FromUser = currentUser.Id,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = createdBy,
                    VisibleToApplicant = true,
                    IncludeInReporting = true,
                    Comment = applicationResponseCommentItem.Comment,
                    CommentTypeId = applicationResponseCommentItem.CommentType.Id,
                    ApplicationResponseCommentDocuments =
                        applicationResponseCommentItem.CommentDocuments.Select(
                                x => new ApplicationResponseCommentDocument
                                {
                                    Id = Guid.NewGuid(),
                                    DocumentId = x.DocumentId,
                                    CreatedBy = createdBy,
                                    CreatedDate = DateTime.Now
                                })
                            .ToList()
                };

                //applicationResponseComment.ToUser = userManager.GetByEmailAddress(createdBy).Id;                


                applicationResponseCommentManager.Add(applicationResponseComment);

                applicationResponseComment.CommentFrom = currentUser;

            }
            else //update 
            {
                applicationResponseComment = applicationResponseCommentManager.GetById(applicationResponseCommentItem.ApplicationResponseCommentId);

                
                applicationResponseComment.DocumentId = applicationResponseCommentItem.DocumentId;
                //applicationResponseComment.ToUser = userManager.GetByEmailAddress(createdBy).Id;
                applicationResponseComment.UpdatedDate = DateTime.Now;


                if (currentUserByEmail.Id != applicationResponseComment.FromUser.Value &&
                    (currentUserByEmail.Role.Name == Constants.Roles.FACTAdministrator ||
                     currentUserByEmail.Role.Name == Constants.Roles.FACTCoordinator ||
                     currentUserByEmail.Role.Name == Constants.Roles.QualityManager))
                {
                    applicationResponseComment.CommentOverride = applicationResponseCommentItem.Comment;
                    applicationResponseComment.OverridenBy = createdBy;
                }
                else
                {
                    applicationResponseComment.FromUser = userManager.GetByEmailAddress(createdBy).Id;
                    applicationResponseComment.UpdatedBy = createdBy;
                    applicationResponseComment.Comment = applicationResponseCommentItem.Comment;
                }

                
                applicationResponseComment.CommentTypeId = applicationResponseCommentItem.CommentType.Id;

                applicationResponseComment.ApplicationResponseCommentDocuments =
                    applicationResponseCommentItem.CommentDocuments.Select(
                            x => new ApplicationResponseCommentDocument
                            {
                                Id = Guid.NewGuid(),
                                DocumentId = x.DocumentId,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now
                            })
                        .ToList();

                applicationResponseCommentManager.Save(applicationResponseComment);

            }

            var association = associationTypeManager.GetByName(Constants.AssociationTypes.RfiResponse);

            if (applicationResponseCommentItem.DocumentId.HasValue)
            {
                documentManager.UpdateDocumentAssociations(applicationResponseCommentItem.DocumentId.Value, association, createdBy);

                var doc = documentManager.GetById(applicationResponseCommentItem.DocumentId.Value);
                applicationResponseComment.Document = doc;
            }

            if (applicationResponseCommentItem.CommentDocuments != null &&
                applicationResponseCommentItem.CommentDocuments.Count > 0)
            {
                foreach (var doc in applicationResponseCommentItem.CommentDocuments)
                {
                    documentManager.UpdateDocumentAssociations(doc.DocumentId, association, createdBy);
                }
            }

            return applicationResponseComment;
        }

        public void SetCommentVisibility(int commentId, bool isVisible, string savedBy)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            var comment = applicationResponseCommentManager.GetById(commentId);

            if (comment == null)
            {
                throw new Exception("Cannot find Comment to set Visibility");
            }

            comment.VisibleToApplicant = isVisible;            
            comment.UpdatedBy = savedBy;
            comment.UpdatedDate = DateTime.Now;

            applicationResponseCommentManager.Save(comment);
        }

        public void SetCommentInclusion(int commentId, bool includeInReporting, string savedBy)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();

            var comment = applicationResponseCommentManager.GetById(commentId);

            if (comment == null)
            {
                throw new Exception("Cannot find Comment to set Visibility");
            }
            
            comment.IncludeInReporting = includeInReporting;
            comment.UpdatedBy = savedBy;
            comment.UpdatedDate = DateTime.Now;

            applicationResponseCommentManager.Save(comment);
        }

        public void Delete(int applicationResponseCommentId, bool isFactStaff, string updatedBy)
        {
            var applicationResponseCommentManager = this.container.GetInstance<ApplicationResponseCommentManager>();
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();
            var responseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();
            var applicationResponseCommentDocumentManager =
                this.container.GetInstance<ApplicationResponseCommentDocumentManager>();

            var comment = applicationResponseCommentManager.GetById(applicationResponseCommentId);

            if (comment == null)
            {
                throw new Exception("Cannot find comment");
            }

            if (comment.CreatedBy != updatedBy && !isFactStaff)
            {
                throw new Exception("Not Authorized");
            }

            var responses = applicationResponseManager.GetResponsesByAppIdQuestionId(comment.QuestionId,
                comment.ApplicationId.GetValueOrDefault());

            var responseStatus = responseStatusManager.GetStatusByName(Constants.ApplicationResponseStatus.ForReview);

            foreach (var response in responses)
            {
                if (response.ApplicationResponseStatusId == responseStatus.Id) continue;

                response.ApplicationResponseStatusId = responseStatus.Id;
                response.UpdatedBy = updatedBy;
                response.UpdatedDate = DateTime.Now;

                applicationResponseManager.Save(response);
            }

            applicationResponseCommentDocumentManager.RemoveForComment(applicationResponseCommentId);

            applicationResponseCommentManager.Remove(applicationResponseCommentId);
    
        }


    }
}
