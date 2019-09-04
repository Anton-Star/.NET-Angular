using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class OrganizationBAADocumentRepository : BaseRepository<OrganizationBAADocument>, IOrganizationBAADocumentRepository
    {
        public OrganizationBAADocumentRepository(FactWebContext context) : base(context)
        {
        }

        public List<OrganizationBAADocument> GetAllByOrganization(int orgId)
        {
            return base.FetchMany(x => x.OrganizationId == orgId);
        }

        public List<OrganizationBAADocument> GetAllByDocument(Guid documentId)
        {
            return base.FetchMany(x => x.DocumentId == documentId);
        }

        public Task<List<OrganizationBAADocument>> GetAllByDocumentAsync(Guid documentId)
        {
            return base.FetchManyAsync(x => x.DocumentId == documentId);
        }
    }
}
