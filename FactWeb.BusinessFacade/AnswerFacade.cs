using FactWeb.BusinessLayer;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class AnswerFacade
    {
        private readonly Container container;

        public AnswerFacade(Container container)
        {
            this.container = container;
        }

        public void Save(Answer answer, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();

            manager.AddOrUpdate(answer, updatedBy);
        }

        public async Task<Guid> SaveAsync(Answer answer, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();

            return await manager.AddOrUpdateAsync(answer, updatedBy);
        }

        public void Delete(Guid id, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();

            manager.Remove(id, updatedBy);
        }

        public async Task DeleteAsync(Guid id, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerManager>();

            await manager.RemoveAsync(id, updatedBy);
        }

        public void RemoveHides(Guid id, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

            manager.Remove(id, updatedBy);
        }

        public async Task RemoveHidesAsync(Guid id, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

            await manager.RemoveAsync(id, updatedBy);
        }

        public List<ApplicationSectionQuestionAnswerDisplay> AddHides(Guid answerId, List<Question> questions, string createdBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

            return manager.AddHides(answerId, questions, createdBy);
        }

        public async Task<List<ApplicationSectionQuestionAnswerDisplay>> AddHidesAsync(Guid answerId, List<Question> questions, string createdBy)
        {
            var manager = this.container.GetInstance<ApplicationSectionQuestionAnswerDisplayManager>();

            return await manager.AddHidesAsync(answerId, questions, createdBy);
        }
    }
}
