using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSectionQuestionRepository : IRepository<ApplicationSectionQuestion>
    {
        /// <summary>
        /// Gets an Application Section Question by id
        /// </summary>
        /// <param name="id">Id of the application section question</param>
        /// <returns>ApplicationSectionQuestion object</returns>
        ApplicationSectionQuestion GetById(Guid id);

        List<ApplicationSectionQuestion> GetAllByApplicationType(int applicationTypeId);
        Task<List<ApplicationSectionQuestion>> GetAllByApplicationTypeAsync(int applicationTypeId);

        List<ApplicationSectionQuestion> GetAllForActiveVersionsNoLazyLoad();

        List<ApplicationSectionQuestion> GetAllForVersionNoLazyLoad(Guid versionId);

        List<ApplicationSectionQuestion> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName);

        List<SectionQuestion> GetSectionQuestions(Guid applicationUniqueId, Guid applicationSectionId, Guid userId);
        List<ApplicationSectionQuestion> GetQuestionsWithRFIs(int applicationId);
    }
}
