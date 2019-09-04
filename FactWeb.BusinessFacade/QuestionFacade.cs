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
    public class QuestionFacade
    {
        private readonly Container container;

        public QuestionFacade(Container container)
        {
            this.container = container;
        }

        public List<QuestionType> GetAllQuestionTypes()
        {
            var questionTypeManager = this.container.GetInstance<QuestionTypeManager>();

            return questionTypeManager.GetAll();
        }

        public async Task<ApplicationSectionQuestion> SaveAsync(Question question, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionManager>();
            var questionTypeManager = this.container.GetInstance<QuestionTypeManager>();
            var questionScopeManager = this.container.GetInstance<ApplicationSectionQuestionScopeTypeManager>();
            var applicationSectionScopeTypeManeger = this.container.GetInstance<ApplicationSectionScopeTypeManager>();
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();
            var applicationFacade = this.container.GetInstance<ApplicationFacade>();

            var questionType = await questionTypeManager.GetByNameAsync(question.Type);
            var version = applicationVersionManager.GetByApplicationSection(question.SectionId.GetValueOrDefault());
            var newStatus = applicationResponseStatusManager.GetStatusByName(Constants.ApplicationResponseStatus.New);

            await questionScopeManager.RemoveItemsAsync(question.Id.GetValueOrDefault());

            var applicaitonSectionQuestion = await manager.AddOrUpdateAsync(version, questionType, question, updatedBy);
            await applicationSectionScopeTypeManeger.AddOrUpdateApplicationSectionScopeTypeAsync(question, updatedBy);

            var statuses = new List<string>
            {
                Constants.ApplicationStatus.Cancelled,
                Constants.ApplicationStatus.Declined,
                Constants.ApplicationStatus.InProgress,
                Constants.ApplicationStatus.Applied,
                Constants.ApplicationStatus.Approved,
                Constants.ApplicationStatus.Complete
            };

            var apps = version.Applications.Where(x => statuses.All(y => y != x.ApplicationStatus.Name));

            foreach (var app in apps)
            {
                if (app.ApplicationResponses.Any(x => x.ApplicationSectionQuestionId == applicaitonSectionQuestion.Id))
                    continue;

                app.ApplicationResponses.Add(new ApplicationResponse
                {
                    ApplicationSectionQuestionId = applicaitonSectionQuestion.Id,
                    Flag = false,
                    ApplicationResponseStatusId = newStatus.Id,
                    VisibleApplicationResponseStatusId = newStatus.Id,
                    CreatedDate = DateTime.Now,
                    CreatedBy = updatedBy,
                });
            }

            applicationVersionManager.Save(version);

            return applicaitonSectionQuestion;
        }

        public void Delete(Guid id, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionManager>();

            manager.Remove(id, updatedBy);
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllDisplaysForQuestion(Guid questionId)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

            return manager.GetAllForQuestion(questionId);
        }
    }
}
