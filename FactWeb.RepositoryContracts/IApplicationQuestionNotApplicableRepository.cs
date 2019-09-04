using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationQuestionNotApplicableRepository : IRepository<ApplicationQuestionNotApplicable>
    {
        List<ApplicationQuestionNotApplicable> GetByOrganization(int organizationId);
        Task<List<ApplicationQuestionNotApplicable>> GetByOrganizationAsync(int organizationId);

        List<ApplicationQuestionNotApplicable> GetByApplication(int applicationId);

        List<ApplicationQuestionNotApplicable> GetAllForActiveVersionNoLazyLoad();

        List<ApplicationQuestionNotApplicable> GetAllForVersionNoLazyLoad(Guid versionId);

        List<ApplicationQuestionNotApplicable> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName);

        List<ApplicationQuestionNotApplicable> GetAllForCompliance(Guid complianceApplicationId);

        void RemoveForCompliance(Guid complianceApplicationId, string removedBy);
        void AddForApplication(int applicationId, string questionIds, string createdBy);
    }
}
