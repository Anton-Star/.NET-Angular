using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class OrganizationDocumentRepository : BaseRepository<OrganizationDocument>, IOrganizationDocumentRepository
    {
        public OrganizationDocumentRepository(FactWebContext context) : base(context)
        {
        }

        public OrganizationDocument GetByOrganizationAndDocument(int organizationId, Guid documentId)
        {
            return base.Fetch(x => x.OrganizationId == organizationId && x.DocumentId == documentId);
        }

        public Task<OrganizationDocument> GetByOrganizationAndDocumentAsync(int organizationId, Guid documentId)
        {
            return base.FetchAsync(x => x.OrganizationId == organizationId && x.DocumentId == documentId);
        }

        public List<OrganizationDocument> GetAllByDocument(Guid documentId)
        {
            return base.FetchMany(x => x.DocumentId == documentId);
        }

        public Task<List<OrganizationDocument>> GetAllByDocumentAsync(Guid documentId)
        {
            return base.FetchManyAsync(x => x.DocumentId == documentId);
        }
    }
}
