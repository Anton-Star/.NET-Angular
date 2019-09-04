using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class OrganizationBAADocumentManager : BaseManager<OrganizationBAADocumentManager, IOrganizationBAADocumentRepository, OrganizationBAADocument>
    {
        public OrganizationBAADocumentManager(IOrganizationBAADocumentRepository repository) : base(repository)
        {
        }

        public List<OrganizationBAADocument> GetByOrganization(int organizationId)
        {
            LogMessage("GetByOrganization (OrganizationBAADocumentManager)");

            return base.Repository.GetAllByOrganization(organizationId);
        }

        public async Task<bool> RemoveAsync(int organizationId, Guid documentId)
        {
            LogMessage("RemoveAsync (OrganizationBAADocumentManager)");

            var docs = await this.Repository.GetAllByDocumentAsync(documentId);

            if (docs.Count == 0) return true;

            var doc = docs.SingleOrDefault(x => x.OrganizationId == organizationId);

            if (doc == null) return false;

            await this.Repository.RemoveAsync(doc);

            return docs.Count <= 1;
        }
    }
}
