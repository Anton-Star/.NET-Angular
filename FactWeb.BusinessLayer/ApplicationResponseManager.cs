using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationResponseManager : BaseManager<ApplicationResponseManager, IApplicationResponseRepository, ApplicationResponse>
    {
        public ApplicationResponseManager(IApplicationResponseRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the application responses for an organization and application type
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of application responses</returns>
        public List<ApplicationResponse> GetApplicationResponses(long organizationId, int applicationTypeId)
        {
            LogMessage("GetApplicationResponses (ApplicationResponseManager)");

            return this.Repository.GetApplicationResponses(organizationId, applicationTypeId);
        }

        /// <summary>
        /// Gets the application responses for an organization and application type asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of application responses</returns>
        public Task<List<ApplicationResponse>> GetApplicationResponsesAsync(long organizationId, int applicationTypeId)
        {
            LogMessage("GetApplicationResponsesAsync (ApplicationResponseManager)");

            return this.Repository.GetApplicationResponsesAsync(organizationId, applicationTypeId);
        }

        public List<ApplicationResponse> GetApplicationResponses(Guid applicationUniqueId)
        {
            return this.Repository.GetApplicationResponses(applicationUniqueId);
        }

        ///// <summary>
        ///// Removes a document from the responses
        ///// </summary>
        ///// <param name="organizationId">Id of the organization</param>
        ///// <param name="documentId">Id of the document</param>
        ///// <param name="updatedBy">Whos doing the update</param>
        //public void RemoveDocument(int organizationId, Guid documentId, string updatedBy)
        //{
        //    var responses = this.Repository.GetApplicationResponsesWithDocuments(organizationId, documentId);

        //    //Todo new method to be created

        //    //foreach (var response in responses)
        //    //{
        //    //    response.DocumentId = null;
        //    //    response.UpdatedBy = updatedBy;
        //    //    response.UpdatedDate = DateTime.Now;

        //    //    base.Repository.BatchSave(response);
        //    //}

        //    base.Repository.SaveChanges();
        //}

        ///// <summary>
        ///// Removes a document from the responses asynchronously
        ///// </summary>
        ///// <param name="organizationId">Id of the organization</param>
        ///// <param name="documentId">Id of the document</param>
        ///// <param name="updatedBy">Whos doing the update</param>
        //public async Task RemoveDocumentAsync(int organizationId, Guid documentId, string updatedBy)
        //{
        //    var responses = this.Repository.GetApplicationResponsesWithDocuments(organizationId, documentId);

        //    //Todo new method to be created
        //    //foreach (var response in responses)
        //    //{
        //    //    response.DocumentId = null;
        //    //    response.UpdatedBy = updatedBy;
        //    //    response.UpdatedDate = DateTime.Now;

        //    //    base.Repository.BatchSave(response);
        //    //}

        //    await base.Repository.SaveChangesAsync();
        //}

        public List<ApplicationResponse> GetApplicationResponsesWithDocuments(int organizationId, Guid documentId)
        {
            return this.Repository.GetApplicationResponsesWithDocuments(organizationId, documentId);
        }

        public List<ApplicationResponse> GetApplicationResponses(int organizationId, int applicationResponseStatusId)
        {
            return this.Repository.GetApplicationResponses(organizationId, applicationResponseStatusId);
        }

        public List<ApplicationResponse> GetApplicationRfis(int organizationId)
        {
            return this.Repository.GetApplicationRfis(organizationId);
        }

        /// <summary>
        /// Removes all responses for an organization and application type
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        public void RemoveAll(long organizationId, int applicationTypeId)
        {
            LogMessage("RemoveAll (ApplicationResponseManager)");

            var responses = this.GetApplicationResponses(organizationId, applicationTypeId);

            foreach (var response in responses)
            {
                this.Repository.BatchRemove(response);
            }

            this.Repository.SaveChanges();
        }

        /// <summary>
        /// Removes all responses for an organization and application type asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="applicationTypeId">Id of the application type</param>
        public async Task RemoveAllAsync(long organizationId, int applicationTypeId)
        {
            LogMessage("RemoveAllAsync (ApplicationResponseManager)");

            var responses = await this.GetApplicationResponsesAsync(organizationId, applicationTypeId);

            foreach (var response in responses)
            {
                this.Repository.BatchRemove(response);
            }

            await this.Repository.SaveChangesAsync();
        }

        public void Remove(Guid applicationUniqueId, ApplicationSectionResponse section)
        {
            LogMessage($"Remove (ApplicationResponseManager) - ApplicationUniqueId: {applicationUniqueId}; SectionId: {section.ApplicationSectionId}");

            this.RemoveForSection(applicationUniqueId, section.ApplicationSectionId);

            //var responses = this.GetApplicationResponses(applicationUniqueId);

            //foreach (var question in section.Questions)
            //{
            //    var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
            //    while (rows.Count > 0)
            //    {
            //        this.Repository.BatchRemove(rows[0]);
            //        rows.RemoveAt(0);
            //    }
            //}

            //this.Repository.SaveChanges();

            if (section.Children == null) return;

            foreach (var item in section.Children)
            {
                this.Remove(applicationUniqueId, item);
            }
        }

        //public void Remove(long organizationId, int applicationTypeId, ApplicationSectionItem section)
        //{
        //    LogMessage($"Remove (ApplicationResponseManager) - organizationId: {organizationId}; AppUniqueId: {section.AppUniqueId}; SectionId: {section.Id}");

        //    var responses = this.GetApplicationResponses(organizationId, applicationTypeId);

        //    foreach (var question in section.Questions)
        //    {
        //        var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
        //        while (rows.Count > 0)
        //        {
        //            this.Repository.BatchRemove(rows[0]);
        //            rows.RemoveAt(0);
        //        }
        //    }

        //    this.Repository.SaveChanges();

        //    if (section.Children == null) return;

        //    foreach (var item in section.Children)
        //    {
        //        this.Remove(organizationId, applicationTypeId, item);
        //    }
        //}

        //public async Task RemoveAsync(long organizationId, int applicationTypeId, ApplicationSectionItem section)
        //{
        //    LogMessage("RemoveAsync (ApplicationResponseManager)");

        //    var responses = await this.GetApplicationResponsesAsync(organizationId, applicationTypeId);

        //    foreach (var question in section.Questions)
        //    {
        //        var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
        //        while (rows.Count > 0)
        //        {
        //            this.Repository.BatchRemove(rows[0]);
        //            rows.RemoveAt(0);
        //        }
        //    }

        //    await this.Repository.SaveChangesAsync();

        //    if (section.Children == null) return;

        //    foreach (var item in section.Children)
        //    {
        //        await this.RemoveAsync(organizationId, applicationTypeId, item);
        //    }
        //}

        public void UpdateAnswerResponseStatus(List<ApplicationSectionResponse> applicationSectionItemList, string updatedBy)
        {
            LogMessage("UpdateAnswerResponseStatus (ApplicationResponseManager)");

            foreach (var applicationSectionItem in applicationSectionItemList)
            {
                foreach (var question in applicationSectionItem.Questions)
                {
                    foreach (var questionResponse in question.QuestionResponses)
                    {
                        if (question.AnswerResponseStatusId == 0 || questionResponse.Id == 0)
                            continue;

                        var response = this.Repository.GetById(questionResponse.Id);
                        response.ApplicationResponseStatusId = question.AnswerResponseStatusId;
                        response.UpdatedBy = updatedBy;
                        response.UpdatedDate = DateTime.Now;

                        this.Repository.Save(response);
                    }
                }
            }

            //var responses = this.GetApplicationResponses(organizationId, applicationTypeId);

            //foreach (var question in section.Questions)
            //{
            //    var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
            //    while (rows.Count > 0)
            //    {
            //        this.Repository.BatchRemove(rows[0]);
            //        rows.RemoveAt(0);
            //    }
            //}

            //this.Repository.SaveChanges();

            //if (section.Children == null) return;

            //foreach (var item in section.Children)
            //{
            //    this.Remove(organizationId, applicationTypeId, item);
            //}
        }

        public void UpdateResponseStatus(ApplicationSectionItem section, int fromStatus, int toStatus, string updatedBy, int applicationId)
        {
            LogMessage("UpdateResponseStatus (ApplicationResponseManager)");

            foreach (var question in section.Questions)
            {
                var savedResponses = this.GetResponsesByAppIdQuestionId(question.Id, applicationId);
                foreach (var questionResponse in savedResponses)
                {
                    if ((questionResponse.ApplicationResponseStatusId == null && fromStatus == (int)Constants.ApplicationResponseStatuses.ForReview) || questionResponse.ApplicationResponseStatusId == fromStatus) 
                    {
                        var response = this.Repository.GetById(questionResponse.Id);
                        response.ApplicationResponseStatusId = toStatus;
                        response.UpdatedBy = updatedBy;
                        response.UpdatedDate = DateTime.Now;

                        this.Repository.Save(response);
                    }
                }
            }
        }

        public void UpdateResponseStatus(List<ApplicationResponse> responses, int fromStatus, int toStatus,
            string updatedBy)
        {
            foreach (var questionResponse in responses)
            {
                if ((questionResponse.ApplicationResponseStatusId == null && fromStatus == (int)Constants.ApplicationResponseStatuses.ForReview) || questionResponse.ApplicationResponseStatusId == fromStatus)
                {
                    var response = this.Repository.GetById(questionResponse.Id);
                    response.ApplicationResponseStatusId = toStatus;
                    response.UpdatedBy = updatedBy;
                    response.UpdatedDate = DateTime.Now;

                    this.Repository.Save(response);
                }
            }
        }

        /// <summary>
        /// Get all responses against a question and application id
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public List<ApplicationResponse> GetResponsesByAppIdQuestionId(Guid? questionId, int applicationId)
        {
            LogMessage("GetResponsesByAppIdQuestionId (ApplicationResponseManager)");

            var applicationResponses = this.Repository.GetResponsesByAppIdQuestionId(questionId, applicationId);

            return applicationResponses;
        }

        //public void UpdateSection(List<ApplicationSectionItem> applicationSectionItemList, string updatedBy)
        //{
        //    LogMessage("UpdateSection (ApplicationResponseManager)");

        //    foreach (var applicationSectionItem in applicationSectionItemList)
        //    {
        //        foreach (var question in applicationSectionItem.Questions)
        //        {                    
        //            foreach (var questionResponse in question.QuestionResponses)
        //            {
        //                if (question.AnswerResponseStatusId == 0)
        //                    continue;

        //                var response = this.Repository.GetById(questionResponse.Id);
        //                response.ApplicationResponseStatusId = question.AnswerResponseStatusId;
        //                response.CoorindatorComment = questionResponse.CoordinatorComment;

        //                this.Repository.Save(response);
        //            }

        //            foreach (var comment in question.ApplicationResponseComments)
        //            {                        
        //                var response = this.Repository.GetById(questionResponse.Id);
        //                response.ApplicationResponseStatusId = question.AnswerResponseStatusId;
        //                response.CoorindatorComment = questionResponse.CoordinatorComment;

        //                this.Repository.Save(response);
        //            }


        //        }
        //    }

        //    //var responses = this.GetApplicationResponses(organizationId, applicationTypeId);

        //    //foreach (var question in section.Questions)
        //    //{
        //    //    var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
        //    //    while (rows.Count > 0)
        //    //    {
        //    //        this.Repository.BatchRemove(rows[0]);
        //    //        rows.RemoveAt(0);
        //    //    }
        //    //}

        //    //this.Repository.SaveChanges();

        //    //if (section.Children == null) return;

        //    //foreach (var item in section.Children)
        //    //{
        //    //    this.Remove(organizationId, applicationTypeId, item);
        //    //}
        //}

        public void AddToHistory(Guid applicationUniqueId, Guid sectionId)
        {
            base.Repository.AddToHistory(applicationUniqueId, sectionId);
        }

        public List<ApplicationResponse> GetAllForCompliance(Guid complianceId)
        {
            return base.Repository.GetAllForCompliance(complianceId);
        }

        public void BulkUpdate(int applicationId, Guid applicationSectionId, int fromStatusId, int toStatusId, string updatedBy)
        {
            base.Repository.BulkUpdate(applicationId, applicationSectionId, fromStatusId, toStatusId, updatedBy);
        }

        public void RemoveForSection(Guid applicationUniqueId, Guid sectionId)
        {
            base.Repository.RemoveForSection(applicationUniqueId, sectionId);
        }

        public void ProcessExpectedAndHidden(Guid applicationUniqueId, string updatedBy)
        {
            base.Repository.ProcessExpectedAndHidden(applicationUniqueId, updatedBy);
        }
    }
}
