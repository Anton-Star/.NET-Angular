using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationDocumentRepository : IRepository<OrganizationDocument>
    {

        OrganizationDocument GetByOrganizationAndDocument(int organizationId, Guid documentId);
        Task<OrganizationDocument> GetByOrganizationAndDocumentAsync(int organizationId, Guid documentId);

        List<OrganizationDocument> GetAllByDocument(Guid documentId);
        Task<List<OrganizationDocument>> GetAllByDocumentAsync(Guid documentId);
    }
}
