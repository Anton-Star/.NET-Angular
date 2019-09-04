using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationQuestionNotApplicableManager : BaseManager<ApplicationQuestionNotApplicableManager, IApplicationQuestionNotApplicableRepository, ApplicationQuestionNotApplicable>
    {
        public ApplicationQuestionNotApplicableManager(IApplicationQuestionNotApplicableRepository repository) : base(repository)
        {
        }

        public List<ApplicationQuestionNotApplicable> GetByOrganization(int organizationId)
        {
            return base.Repository.GetByOrganization(organizationId);
        }

        public Task<List<ApplicationQuestionNotApplicable>> GetByOrganizationAsync(int organizationId)
        {
            return base.Repository.GetByOrganizationAsync(organizationId);
        }

        public void RemoveComplianceForOrganization(int organizationId)
        {
            var items = this.GetByOrganization(organizationId);

            foreach (var item in items)
            {
                base.Repository.BatchRemove(item);
            }

            base.Repository.SaveChanges();
        }

        public async Task RemoveComplianceForOrganizationAsync(int organizationId)
        {
            var items = await this.GetByOrganizationAsync(organizationId);

            foreach (var item in items)
            {
                base.Repository.BatchRemove(item);
            }

            await base.Repository.SaveChangesAsync();
        }

        public List<ApplicationQuestionNotApplicable> GetByApplication(int applicationId)
        {
            return base.Repository.GetByApplication(applicationId);
        }

        public List<ApplicationQuestionNotApplicable> GetAllForActiveVersionNoLazyLoad()
        {
            return base.Repository.GetAllForActiveVersionNoLazyLoad();
        }

        public List<ApplicationQuestionNotApplicable> GetAllForVersionNoLazyLoad(Guid versionId)
        {
            return base.Repository.GetAllForVersionNoLazyLoad(versionId);
        }

        public List<ApplicationQuestionNotApplicable> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName)
        {
            return base.Repository.GetAllForApplicationTypeNoLazyLoad(applicationTypeName);
        }

        public List<ApplicationQuestionNotApplicable> GetAllForCompliance(Guid complianceApplicationId)
        {
            return base.Repository.GetAllForCompliance(complianceApplicationId);
        }

        public void RemoveForCompliance(Guid complianceApplicationId, string removedBy)
        {
            base.Repository.RemoveForCompliance(complianceApplicationId, removedBy);
        }

        public void AddForApplication(int applicationId, string questionIds, string createdBy)
        {
            base.Repository.AddForApplication(applicationId, questionIds, createdBy);
        }
    }
}
