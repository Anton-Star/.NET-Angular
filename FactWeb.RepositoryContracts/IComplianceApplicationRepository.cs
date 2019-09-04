using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IComplianceApplicationRepository : IRepository<ComplianceApplication>
    {
        List<ComplianceApplication> GetByOrg(int orgId);
        Task<List<ComplianceApplication>> GetByOrgAsync(int orgId);
        ComplianceApplication GetByIdInclusive(Guid id);
        ComplianceApplication GetByIdSemiInclusive(Guid id);

        List<CbTotal> GetCbTotals(Guid complianceApplicationId);
        List<CtTotal> GetCtTotals(Guid complianceApplicationId);

        DoesInspectorHaveAccessModel DoesInspectorHaveAccess(Guid? applicationUniqueId, Guid? complianceApplicationId, Guid userId);
        void UpdateComplianceApplicationStatus(Guid complianceApplicationId, string statusName, string updatedBy);
        ComplianceApplication GetByIdWithApps(Guid id);
        void CreateComplianceApplicationSections(Guid complianceApplicationId);
        bool AppHasRfis(Guid complianceApplicationId);
    }
}
