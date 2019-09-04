using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSectionQuestionAnswerDisplayManager : BaseManager<ApplicationSectionQuestionAnswerDisplayManager, IApplicationSectionQuestionAnswerDisplayRepository, ApplicationSectionQuestionAnswerDisplay>
    {
        public ApplicationSectionQuestionAnswerDisplayManager(IApplicationSectionQuestionAnswerDisplayRepository repository) : base(repository)
        {
        }

        public void Remove(Guid id, string updatedBy)
        {
            LogMessage("Remove (ApplicationSectionQuestionAnswerDisplayManager)");

            var item = this.GetById(id);

            if (item == null)
            {
                throw new KeyNotFoundException("Cannot find id");
            }

            base.Repository.Remove(item);
        }

        public async Task RemoveAsync(Guid id, string updatedBy)
        {
            LogMessage("RemoveAsync (ApplicationSectionQuestionAnswerDisplayManager)");

            var item = this.GetById(id);

            if (item == null)
            {
                throw new KeyNotFoundException("Cannot find id");
            }

            await base.Repository.RemoveAsync(item);
        }

        public List<ApplicationSectionQuestionAnswerDisplay> AddHides(Guid answerId, List<Question> questions, string createdBy)
        {
            var result = new List<ApplicationSectionQuestionAnswerDisplay>();

            foreach (var question in questions)
            {
                if (!question.Id.HasValue) continue;

                var display = new ApplicationSectionQuestionAnswerDisplay
                {
                    Id = Guid.NewGuid(),
                    ApplicationSectionQuestionAnswerId = answerId,
                    HidesQuestionId = question.Id.Value,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy
                };

                base.Repository.BatchAdd(display);
                result.Add(display);
            }

            base.Repository.SaveChanges();

            return result;
        }

        public async Task<List<ApplicationSectionQuestionAnswerDisplay>> AddHidesAsync(Guid answerId, List<Question> questions, string createdBy)
        {
            var result = new List<ApplicationSectionQuestionAnswerDisplay>();

            foreach (var question in questions)
            {
                if (!question.Id.HasValue) continue;

                var display = new ApplicationSectionQuestionAnswerDisplay
                {
                    Id = Guid.NewGuid(),
                    ApplicationSectionQuestionAnswerId = answerId,
                    HidesQuestionId = question.Id.Value,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy
                };

                base.Repository.BatchAdd(display);
                result.Add(display);
            }

            await base.Repository.SaveChangesAsync();

            return result;
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllForApplicationType(int applicationTypeId)
        {
            return base.Repository.GetAllForApplicationType(applicationTypeId);
        }

        public Task<List<ApplicationSectionQuestionAnswerDisplay>> GetAllForApplicationTypeAsync(int applicationTypeId)
        {
            return base.Repository.GetAllForApplicationTypeAsync(applicationTypeId);
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllForQuestion(Guid questionId)
        {
            return base.Repository.GetAllForQuestion(questionId);
        }

        public List<ApplicationSectionQuestionAnswerDisplay> GetAllForVersion(Guid versionId)
        {
            return base.Repository.GetAllForVersion(versionId);
        }

        public List<QuestionAnswerDisplay> GetDisplaysForSection(Guid applicationSectionId)
        {
            return base.Repository.GetDisplaysForSection(applicationSectionId);
        }
    }
}
