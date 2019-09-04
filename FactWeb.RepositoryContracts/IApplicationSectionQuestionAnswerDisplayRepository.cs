using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSectionQuestionAnswerDisplayRepository : IRepository<ApplicationSectionQuestionAnswerDisplay>
    {
        List<ApplicationSectionQuestionAnswerDisplay> GetAllForApplicationType(int applicationTypeId);
        Task<List<ApplicationSectionQuestionAnswerDisplay>> GetAllForApplicationTypeAsync(int applicationTypeId);
        List<ApplicationSectionQuestionAnswerDisplay> GetAllForQuestion(Guid questionId);
        List<ApplicationSectionQuestionAnswerDisplay> GetAllForVersion(Guid versionId);
        List<QuestionAnswerDisplay> GetDisplaysForSection(Guid applicationSectionId);
    }
}
