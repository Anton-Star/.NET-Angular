using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class OrganizationDocumentManager : BaseManager<OrganizationDocumentManager, IOrganizationDocumentRepository, OrganizationDocument>
    {
        public OrganizationDocumentManager(IOrganizationDocumentRepository repository) : base(repository)
        {
        }

        public void Add(int organizationId, Guid documentId, string createdBy)
        {
            LogMessage("Add (OrganizationDocumentManager)");

            var organizationDocument = new OrganizationDocument
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                OrganizationId = organizationId,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy
            };

            base.Add(organizationDocument);
        }

        public bool Remove(int organizationId, Guid documentId)
        {
            LogMessage("Add (Remove)");

            var docs = this.Repository.GetAllByDocument(documentId);

            if (docs.Count == 0) return true;

            var doc = docs.SingleOrDefault(x => x.OrganizationId == organizationId);

            if (doc == null) return false;

            this.Repository.Remove(doc);

            return docs.Count <= 1;
        }

        public async Task<bool> RemoveAsync(int organizationId, Guid documentId)
        {
            LogMessage("Add (Remove)");

            var docs = await this.Repository.GetAllByDocumentAsync(documentId);

            if (docs.Count == 0) return true;

            var doc = docs.SingleOrDefault(x => x.OrganizationId == organizationId);

            if (doc == null) return false;

            await this.Repository.RemoveAsync(doc);

            return docs.Count <= 1;
        }
    }
}
