using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationBAADocumentRepository : IRepository<OrganizationBAADocument>
    {
        List<OrganizationBAADocument> GetAllByOrganization(int orgId);

        List<OrganizationBAADocument> GetAllByDocument(Guid documentId);

        Task<List<OrganizationBAADocument>> GetAllByDocumentAsync(Guid documentId);

    }
}
