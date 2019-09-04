using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(FactWebContext context) : base(context)
        {
        }

        public List<Document> GetByOrg(int organizationId)
        {
            return
                base.FetchMany(
                    x =>
                        x.OrganizationDocuments.Any(y => y.OrganizationId == organizationId) &&
                        x.OrganizationDocumentLibrary.IsCurrent);
        }

        public Task<List<Document>> GetByOrgAsync(int organizationId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.OrganizationDocuments.Any(y => y.OrganizationId == organizationId) &&
                        x.OrganizationDocumentLibrary.IsCurrent);
        }

        public List<Document> GetByOrg(string orgName)
        {
            return base.Context.Documents
                .Include(x => x.AssociationTypes)
                .Include(x => x.AssociationTypes.Select(y => y.AssociationType))
                .Where(x => x.OrganizationDocuments.Any(y => y.Organization.Name == orgName) &&
                            x.OrganizationDocumentLibrary.IsCurrent)
                .ToList();
        }

        public Task<List<Document>> GetByOrgAsync(string orgName)
        {
            return base.Context.Documents
                .Include(x => x.AssociationTypes)
                .Include(x=>x.AssociationTypes.Select(y=>y.AssociationType))
                .Where(x => x.OrganizationDocuments.Any(y => y.Organization.Name == orgName) &&
                            x.OrganizationDocumentLibrary.IsCurrent)
                .ToListAsync();
        }

        public List<Document> GetByDocumentLibrary(Guid documentLibraryId)
        {
            return base.Context.Documents
                .Include(x => x.AssociationTypes)
                .Include(x => x.AssociationTypes.Select(y => y.AssociationType))
                .Include(x=>x.ApplicationResponses)
                .Include(x=>x.OrganizationDocumentLibrary)
                .Where(x => x.OrganizationDocumentLibraryId == documentLibraryId)
                .ToList();
        }

        /// <summary>
        /// Get BAA Documents By OrgId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Task<List<Document>> GetBAAByOrgAsync(int organizationId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.OrganizationBAADocuments.Any(y => y.OrganizationId == organizationId) &&
                        x.OrganizationDocumentLibrary.IsCurrent);
        }
        
        /// <summary>
        /// Get BAA Documents By OrgName
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public Task<List<Document>> GetBAAByOrgAsync(string orgName)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.OrganizationBAADocuments.Any(y => y.Organization.Name == orgName) &&
                        x.OrganizationDocumentLibrary.IsCurrent);
        }

        public List<Document> GetByOrgCycle(int organizationId, int cycleNumber)
        {
            return
                base.FetchMany(
                    x =>
                        x.OrganizationDocuments.Any(y => y.OrganizationId == organizationId) &&
                        x.OrganizationDocumentLibrary.CycleNumber == cycleNumber);
        }

        public Task<List<Document>> GetByOrgCycleAsync(int organizationId, int cycleNumber)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.OrganizationDocuments.Any(y => y.OrganizationId == organizationId) &&
                        x.OrganizationDocumentLibrary.CycleNumber == cycleNumber);
        }

        public List<Document> GetPostInspection(string orgName)
        {
            return base.FetchMany(x => x.OrganizationDocuments.Any(y => y.Organization.Name == orgName) &&
                                       x.OrganizationDocumentLibrary.IsCurrent &&
                                       x.DocumentType.Name == Constants.DocumentTypes.PostInspectionEvidence);
        }

        public OrganizationDocumentLibraryItem GetAccessToken(Guid appUniqueId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = appUniqueId;

            var data = objectContext.ExecuteStoreQuery<OrganizationDocumentLibraryItem>(
                "EXEC usp_getAccessToken @applicationUniqueId={0}", paramList).FirstOrDefault();

            return data;
        }

        public void Delete(Guid documentId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = documentId;

            objectContext.ExecuteStoreCommand(
                "EXEC usp_deleteDocument @DocumentId={0}", paramList);
        }
    }
}
