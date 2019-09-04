using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class DocumentManager : BaseManager<DocumentManager, IDocumentRepository, Document>
    {
        public DocumentManager(IDocumentRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Documents</returns>
        public List<Document> GetByOrg(int organizationId)
        {
            LogMessage("GetByOrg (DocumentManager)");

            return base.Repository.GetByOrg(organizationId);
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Documents</returns>
        public Task<List<Document>> GetByOrgAsync(int organizationId)
        {
            LogMessage("GetByOrgAsync (DocumentManager)");

            return base.Repository.GetByOrgAsync(organizationId);
        }

        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public List<Document> GetByOrg(int organizationId, int userRoleId)
        {
            LogMessage("GetByOrg (DocumentManager)");

            var documents = base.Repository.GetByOrg(organizationId);

            if (userRoleId != (int)Constants.Role.FACTAdministrator)
            {
                documents = documents.Where(x => !x.FactStaffOnly).ToList();
            }

            return documents;
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public async Task<List<Document>> GetByOrgAsync(int organizationId, int userRoleId)
        {
            LogMessage("GetByOrgAsync (DocumentManager)");

            var documents = await base.Repository.GetByOrgAsync(organizationId);

            if (userRoleId != (int)Constants.Role.FACTAdministrator)
            {
                documents = documents.Where(x => !x.FactStaffOnly).ToList();
            }

            return documents;
        }

        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public List<Document> GetByOrg(string orgName, int userRoleId)
        {
            LogMessage("GetByOrg (DocumentManager)");

            var documents = base.Repository.GetByOrg(orgName);

            if (userRoleId != (int)Constants.Role.FACTAdministrator)
            {
                documents = documents.Where(x => !x.FactStaffOnly).ToList();
            }

            return documents;
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="canSeeFactOnly">If user can see fact only documents</param>
        /// <returns>Collection of Documents</returns>
        public async Task<List<Document>> GetByOrgAsync(string orgName, bool canSeeFactOnly)
        {
            LogMessage("GetByOrgAsync (DocumentManager)");

            var documents = await base.Repository.GetByOrgAsync(orgName);

            if (!canSeeFactOnly)
            {
                documents = documents.Where(x => !x.FactStaffOnly).ToList();
            }

            return documents;
        }

        public List<Document> GetByDocumentLibrary(Guid documentLibraryId)
        {
            return base.Repository.GetByDocumentLibrary(documentLibraryId);
        }

        public List<Document> GetPostInspection(string orgName, int userRoleId)
        {
            var documents = base.Repository.GetPostInspection(orgName);

            if (userRoleId != (int)Constants.Role.FACTAdministrator)
            {
                documents = documents.Where(x => !x.FactStaffOnly).ToList();
            }

            return documents;
        }

        /// <summary>
        /// Gets a list of baa documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public async Task<List<Document>> GetBAAByOrgAsync(string orgName)
        {
            LogMessage("GetBAAByOrgAsync (DocumentManager)");

            var documents = await base.Repository.GetBAAByOrgAsync(orgName);
            
            return documents;
        }

        /// <summary>
        /// Updates the documents associations
        /// </summary>
        /// <param name="documentId">Id of the document</param>
        /// <param name="association">AssociationType entity object</param>
        /// <param name="savedBy">Who is doing the add</param>
        public void UpdateDocumentAssociations(Guid documentId, AssociationType association, string savedBy)
        {
            var document = this.Repository.GetById(documentId);

            if (document == null) return;

            if (document.AssociationTypes.Any(x => x.AssociationTypeId == association.Id)) return;

            if (document.AssociationTypes == null) document.AssociationTypes = new List<DocumentAssociationType>();

            document.AssociationTypes.Add(new DocumentAssociationType
            {                
                AssociationTypeId = association.Id,
                CreatedDate = DateTime.Now,
                CreatedBy = savedBy,
                Id = Guid.NewGuid()
            });

            base.Repository.Save(document);
        }

        public OrganizationDocumentLibraryItem GetAccessToken(Guid appUniqueId)
        {
            return base.Repository.GetAccessToken(appUniqueId);
        }

        public void Delete(Guid documentId)
        {
            base.Repository.Delete(documentId);
        }
    }
}
