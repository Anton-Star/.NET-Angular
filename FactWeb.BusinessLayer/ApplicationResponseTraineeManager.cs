using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactWeb.BusinessLayer
{
    public class ApplicationResponseTraineeManager : BaseManager<ApplicationResponseTraineeManager, IApplicationResponseTraineeRepository, ApplicationResponseTrainee>
    {
        private readonly ApplicationResponseManager applicationResponseManager;

        public ApplicationResponseTraineeManager(ApplicationResponseManager applicationResponseManager, IApplicationResponseTraineeRepository repository) : base(repository)
        {
            this.applicationResponseManager = applicationResponseManager;
        }

        public void Remove(long organizationId, int applicationTypeId, ApplicationSectionResponse section)
        {
            LogMessage("Remove (ApplicationResponseTraineeManager)");

            var responses = this.GetApplicationResponsesTrainee(organizationId, applicationTypeId);

            foreach (var question in section.Questions)
            {
                var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
                while (rows.Count > 0)
                {
                    this.Repository.BatchRemove(rows[0]);
                    rows.RemoveAt(0);
                }
            }

            this.Repository.SaveChanges();

            if (section.Children == null) return;

            foreach (var item in section.Children)
            {
                this.Remove(organizationId, applicationTypeId, item);
            }
        }

        public void Remove(Guid applicationUniqueId, ApplicationSectionResponse section)
        {
            var responses = this.GetApplicationResponses(applicationUniqueId);

            foreach (var question in section.Questions)
            {
                var rows = responses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
                while (rows.Count > 0)
                {
                    this.Repository.BatchRemove(rows[0]);
                    rows.RemoveAt(0);
                }
            }

            this.Repository.SaveChanges();

            if (section.Children == null) return;

            foreach (var item in section.Children)
            {
                this.Remove(applicationUniqueId, item);
            }
        }

        public List<ApplicationResponseTrainee> GetApplicationResponsesTrainee(long organizationId, int applicationTypeId)
        {
            LogMessage("GetApplicationResponsesTrainee (ApplicationResponseTraineeManager)");

            return this.Repository.GetApplicationResponsesTrainee(organizationId, applicationTypeId);
        }

        public List<ApplicationResponseTrainee> GetApplicationResponses(Guid applicationUniqueId)
        {
            return this.Repository.GetApplicationResponses(applicationUniqueId);
        }

        public void UpdateResponseStatus(ApplicationSectionItem section, int fromStatus, int toStatus, string updatedBy, Guid applicationUniqueId, int applicationId)
        {
            LogMessage("UpdateResponseStatus (ApplicationResponseTraineeManager)");

            var traineeResponses = this.GetApplicationResponses(applicationUniqueId);

            foreach (var question in section.Questions.Where(x=>x.Id != null))
            {
                var savedResponses = traineeResponses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();

                if (savedResponses.Count == 0 && fromStatus == (int) Constants.ApplicationResponseStatuses.Reviewed)
                {
                    var actualResponses = this.applicationResponseManager.GetResponsesByAppIdQuestionId(question.Id, applicationId);

                    foreach (var actualResponse in actualResponses)
                    {
                        var resp = new ApplicationResponseTrainee
                        {
                            ApplicationId = applicationId,
                            ApplicationSectionQuestionId = question.Id.Value,
                            ApplicationResponseStatusId = toStatus,
                            CreatedBy = updatedBy,
                            CreatedDate = DateTime.Now
                        };

                        this.Repository.Add(resp);
                    }
                }
                else
                {
                    foreach (var questionResponse in savedResponses)
                    {
                        if (questionResponse.ApplicationResponseStatusId == fromStatus)
                        {
                            questionResponse.ApplicationResponseStatusId = toStatus;

                            this.Repository.Save(questionResponse);
                        }
                    }
                }

                
            }
        }

        public void BulkUpdate(int applicationId, Guid applicationSectionId, int fromStatusId, int toStatusId,
            string updatedBy)
        {
            this.Repository.BulkUpdate(applicationId, applicationSectionId, fromStatusId, toStatusId, updatedBy);
        }
    }
}