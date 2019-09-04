using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class OrganizationAccreditationCycleManager : BaseManager<OrganizationAccreditationCycleManager, IOrganizationAccreditationCycleRepository, OrganizationAccreditationCycle>
    {
        public OrganizationAccreditationCycleManager(IOrganizationAccreditationCycleRepository repository) : base(repository)
        {
        }

        public List<OrganizationAccreditationCycle> GetByOrganization(int organizationId)
        {
            return base.Repository.GetByOrganization(organizationId);
        }

        public Task<List<OrganizationAccreditationCycle>> GetByOrganizationAsync(int organizationId)
        {
            return base.Repository.GetByOrganizationAsync(organizationId);
        }

        public OrganizationAccreditationCycle GetCurrentByOrganization(int organizationId)
        {
            return base.Repository.GetCurrentByOrganization(organizationId);
        }

        public Task<OrganizationAccreditationCycle> GetCurrentByOrganizationAsync(int organizationId)
        {
            return base.Repository.GetCurrentByOrganizationAsync(organizationId);
        }

        public OrganizationAccreditationCycle AddCycle(int organizationId, int cycleNumber, DateTime effectiveDate,
            bool isCurrent, string createdBy)
        {
            var cycle = new OrganizationAccreditationCycle
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                Number = cycleNumber,
                EffectiveDate = effectiveDate,
                IsCurrent = isCurrent
            };

            base.Repository.Add(cycle);
            
            return cycle;
        }

        public async Task<OrganizationAccreditationCycle> AddCycleAsync(int organizationId, int cycleNumber, DateTime effectiveDate,
            bool isCurrent, string createdBy)
        {
            var cycle = new OrganizationAccreditationCycle
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                Number = cycleNumber,
                EffectiveDate = effectiveDate,
                IsCurrent = isCurrent
            };

            await base.Repository.AddAsync(cycle);

            return cycle;
        }

        public OrganizationAccreditationCycle RemoveCurrentAndAddCycle(int organizationId, DateTime effectiveDate, string createdBy, int? cycleNumber = null)
        {
            var current = this.GetCurrentByOrganization(organizationId);

            if (current != null)
            {
                current.IsCurrent = false;
                current.UpdatedBy = createdBy;
                current.UpdatedDate = DateTime.Now;

                base.Repository.Save(current);
            }

            return this.AddCycle(organizationId, cycleNumber.HasValue ? cycleNumber.Value : current?.Number + 1 ?? 1, effectiveDate, true, createdBy);
        }

        public async Task<OrganizationAccreditationCycle> RemoveCurrentAndAddCycleAsync(int organizationId, DateTime effectiveDate, string createdBy)
        {
            var current = await this.GetCurrentByOrganizationAsync(organizationId);

            if (current != null)
            {
                current.IsCurrent = false;
                current.UpdatedBy = createdBy;
                current.UpdatedDate = DateTime.Now;

                base.Repository.Save(current);
            }

            return await this.AddCycleAsync(organizationId, current?.Number + 1 ?? 1, effectiveDate, true, createdBy);
        }
    }
}
