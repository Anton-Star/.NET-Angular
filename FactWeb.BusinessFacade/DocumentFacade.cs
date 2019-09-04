using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.Model.TrueVault;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class DocumentFacade
    {
        private readonly Container container;

        public DocumentFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Documents</returns>
        public List<Document> GetByOrg(int organizationId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetByOrg(organizationId);
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of Documents</returns>
        public Task<List<Document>> GetByOrgAsync(int organizationId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetByOrgAsync(organizationId);
        }

        /// <summary>
        /// Gets a list of documents for an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public List<Document> GetByOrg(int organizationId, int userRoleId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetByOrg(organizationId, userRoleId);
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public Task<List<Document>> GetByOrgAsync(int organizationId, int userRoleId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetByOrgAsync(organizationId, userRoleId);
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="canSeeFactOnly">If the user can see Fact Only documents</param>
        /// <returns>Collection of Documents</returns>
        public Task<List<Document>> GetByOrgAsync(string orgName, bool canSeeFactOnly)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetByOrgAsync(orgName, canSeeFactOnly);
        }

        public bool CanSeeFactOnlyDocuments(string orgName, int userRoleId, Guid userId)
        {
            var detailmanager = this.container.GetInstance<InspectionScheduleDetailManager>();

            return detailmanager.CanSeeFactOnlyDocuments(orgName, userRoleId, userId);
        }

        public List<Document> GetByDocumentLibrary(Guid appId, int userRoleId, Guid userId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var app = applicationManager.GetByUniqueIdIgnoreActive(appId);

            var org = orgManager.GetById(app.OrganizationId);

            var canSeeFactOnly = this.CanSeeFactOnlyDocuments(org.Name, userRoleId, userId);

            var docLibrary =
                org.DocumentLibraries.OrderBy(x => x.CycleNumber)
                    .FirstOrDefault(
                        x =>
                            (app.IsActive.GetValueOrDefault() && x.IsCurrent) ||
                            x.CycleNumber == app.CycleNumber && app.CycleNumber != 0 ||
                            app.CycleNumber == 0 && x.IsCurrent);

            var docs = docLibrary == null ? new List<Document>() : documentManager.GetByDocumentLibrary(docLibrary.Id);

            if (!canSeeFactOnly)
            {
                docs = docs.Where(x => !x.FactStaffOnly).ToList();
            }

            return docs;
        }

        public List<Document> GetPostInspection(string orgName, int userRoleId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetPostInspection(orgName, userRoleId);
        }

        /// <summary>
        /// Gets a list of BAA documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <returns>Collection of Documents</returns>
        public Task<List<Document>> GetBAAByOrgAsync(string orgName)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetBAAByOrgAsync(orgName);
        }

        /// <summary>
        /// Gets a list of documents for an organization asynchronously
        /// </summary>
        /// <param name="orgName">Name of the organization</param>
        /// <param name="userRoleId">Role of the user</param>
        /// <returns>Collection of Documents</returns>
        public List<Document> GetByOrg(string orgName, int userRoleId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            return documentManager.GetByOrg(orgName, userRoleId);
        }

        public void ChangeLatest(Guid documentId, string updatedBy)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            var doc = documentManager.GetById(documentId);

            if (doc != null)
            {
                doc.IsLatestVersion = false;
                doc.UpdatedBy = updatedBy;
                doc.UpdatedDate = DateTime.Now;
                documentManager.Save(doc);
            }

        }

        /// <summary>
        /// Adds a document to the document library and to the document table asynchronously
        /// </summary>
        /// <param name="applicationUniqueId">Id of the application</param>
        /// <param name="organizationName">Name of the organization</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="originalFileName">Original Name of the file</param>
        /// <param name="isFactOnly">Whether or not FACT staff can only see the file or not</param>
        /// <param name="createdBy">Who is creating the document</param>
        /// <param name="requestValues">Json object stringified for True Vault</param>
        /// <param name="includeInLibrary">Whether to include in the Library</param>
        /// <returns></returns>
        public Document AddToLibrary(Guid? applicationUniqueId, string organizationName, string fileName, string originalFileName, bool isFactOnly, string createdBy, string requestValues, bool includeInLibrary = true, bool isBAADocument = false)
        {
           // var documentLibrary = this.container.GetInstance<IDocumentLibrary>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();
            var documentTypeManager = this.container.GetInstance<DocumentTypeManager>();
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();

            var organization = organizationManager.GetByName(organizationName);

            if (organization == null) throw new KeyNotFoundException("Cannot find organization");

            var currentDocLibrary = organization.DocumentLibraries.Single(x => x.IsCurrent);

            fileName = fileName.Replace("\"", "");
            originalFileName = originalFileName?.Replace("\"", "");

            var documentTypes = documentTypeManager.GetAll();
            int? documentTypeId = null;

            if (applicationUniqueId.HasValue)
            {
                var applicationManager = this.container.GetInstance<ApplicationManager>();
                var app = applicationManager.GetByUniqueId(applicationUniqueId.Value);

                if (app != null)
                {
                    var schedules = app.InspectionSchedules;

                    if (app.ComplianceApplicationId.HasValue)
                    {
                        schedules = inspectionScheduleManager.GetAllForCompliance(app.ComplianceApplicationId.Value);
                    }

                    if (schedules.Count > 0)
                    {
                        var inspectionDate = schedules.Max(x => x.StartDate);

                        if (DateTime.Now > inspectionDate)
                        {
                            documentTypeId =
                                documentTypes.Single(x => x.Name == Constants.DocumentTypes.PostInspectionEvidence).Id;
                        }

                    }
                }
                
            }
            
            if (!documentTypeId.HasValue)
            {
                documentTypeId = documentTypes.Single(x => x.Name == Constants.DocumentTypes.PreInspectionEvidence).Id;
            }

            //var documentLibraryDocument = await documentLibrary.AddFileAsync(organization.Name, fileName, file);

            if (!includeInLibrary) return null;

            var document = new Document
            {
                Id = Guid.NewGuid(),
                OrganizationDocumentLibraryId = currentDocLibrary.Id,
                Name = fileName,
                OriginalName = originalFileName,
                FactStaffOnly = isFactOnly,
                RequestValues = requestValues,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                DocumentTypeId = documentTypeId,
                IsLatestVersion = true,
                OrganizationDocuments = isBAADocument ? null : new List<OrganizationDocument>
                {
                    new OrganizationDocument
                    {
                        Id = Guid.NewGuid(),
                        OrganizationId = organization.Id,
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now                        
                    }
                },
                OrganizationBAADocuments = isBAADocument ? new List<OrganizationBAADocument>
                {
                    new OrganizationBAADocument
                    {
                        Id = Guid.NewGuid(),
                        OrganizationId = organization.Id,
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now
                    }
                } : null
            };

            documentManager.Add(document);          

            return document;
        }

        public async Task RemoveDocumentAsync(string orgName, Guid documentId, string updatedBy)
        {
           // var documentLibrary = this.container.GetInstance<IDocumentLibrary>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();
            var organizationDocumentManager = this.container.GetInstance<OrganizationDocumentManager>();
            var applicationResponseManager = this.container.GetInstance<ApplicationResponseManager>();

            var organization = organizationManager.GetByName(orgName);

            if (organization == null) throw new KeyNotFoundException("Cannot find organization");

            var document = documentManager.GetById(documentId);

            if (document == null) throw new KeyNotFoundException("Cannot find Document");
            //var fileName = document.Name;
            //document = null;
            
            var responses = applicationResponseManager.GetApplicationResponsesWithDocuments(organization.Id, documentId);

            if (responses.Count > 0)
            {
                throw new Exception("Unable to delete. This document is attached to a question.");
            }

            //var canDocumentBeDeleted = await organizationDocumentManager.RemoveAsync(organization.Id, documentId);

            //if (!canDocumentBeDeleted) return;

            documentManager.Delete(documentId);

            //await documentLibrary.RemoveAsync(organization.Name, fileName);
        }

        public void SaveIncludeInReporting(string orgName, List<DocumentItem> documents, string updatedBy)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();

            var organization = organizationManager.GetByName(orgName);

            if (organization == null) throw new KeyNotFoundException("Cannot find organization");

            foreach (var document in documents)
            {
                var doc = documentManager.GetById(document.Id);

                if (doc == null || doc.OrganizationDocuments.All(x => x.OrganizationId != organization.Id))
                {
                    throw new Exception($"Cant find document {document.Id}");
                }

                doc.IncludeInReporting = document.IncludeInReporting;
                doc.UpdatedDate = DateTime.Now;
                doc.UpdatedBy = updatedBy;

                documentManager.BatchSave(doc);
            }

            documentManager.SaveChanges();
        }

        public async Task RemoveBAADocumentAsync(string orgName, Guid documentId, string updatedBy)
        {
            //var documentLibrary = this.container.GetInstance<IDocumentLibrary>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();
            var organizationBAADocumentManager = this.container.GetInstance<OrganizationBAADocumentManager>();

            var organization = organizationManager.GetByName(orgName);

            if (organization == null) throw new KeyNotFoundException("Cannot find organization");

            var document = documentManager.GetById(documentId);

            if (document == null) throw new KeyNotFoundException("Cannot find Document");
            //var fileName = document.Name;
            //document = null;

            var canDocumentBeDeleted = await organizationBAADocumentManager.RemoveAsync(organization.Id, documentId);

            if (!canDocumentBeDeleted) return;

            documentManager.Remove(documentId);

            //await documentLibrary.RemoveAsync(organization.Name, fileName);
        }

        public void RemoveDocument(int organizationId, Guid documentId)
        {
            //var documentLibrary = this.container.GetInstance<IDocumentLibrary>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var documentManager = this.container.GetInstance<DocumentManager>();
            var organizationDocumentManager = this.container.GetInstance<OrganizationDocumentManager>();

            var organization = organizationManager.GetById(organizationId);

            if (organization == null) throw new KeyNotFoundException("Cannot find organization");

            var document = documentManager.GetById(documentId);

            if (document == null) throw new KeyNotFoundException("Cannot find Document");
            //var fileName = document.Path;
            //document = null;

            var canDocumentBeDeleted = organizationDocumentManager.Remove(organizationId, documentId);

            if (!canDocumentBeDeleted) return;

            documentManager.Remove(documentId);

            //documentLibrary.Remove(organization.Name, fileName);
        }

        public DocumentDownload GetFile(string organizationName, string fileName, Guid? userId)
        {
            var documentLibrary = this.container.GetInstance<IDocumentLibrary>();

            if (userId.HasValue)
            {
                var userManager = this.container.GetInstance<UserManager>();

                var user = userManager.GetById(userId.Value);

                if (user == null || user.Organizations.All(x => x.Organization.Name != organizationName))
                {
                    throw new Exception("Not Authorized");
                }
            }

            return documentLibrary.GetFile(organizationName, fileName);
        }

        public AccessTokenDetail GetAcessTokenDetail(Guid appId)
        {
            var documentManager = this.container.GetInstance<DocumentManager>();

            var vault = documentManager.GetAccessToken(appId);

            if (vault == null) return null;

            var result = new AccessTokenDetail
            {
                VaultId = vault.VaultId
            };

            return result;
        }

        public AccessTokenDetail GetAccessTokenDetail(string name, Guid userId, bool isFactStaff)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            AccessTokenDetail result = null;

            var user = userManager.GetById(userId);

            if (user == null)
            {
                throw new Exception("Not Authorized");
            }

            var organization = user.Organizations.FirstOrDefault(x => x.Organization.Name == name)?.Organization;

            if (!isFactStaff)
            {
                if (organization == null)
                {
                    organization = organizationManager.GetByName(name);

                    if (user.Role.Name == Constants.Roles.Inspector)
                    {
                        var detail = inspectionScheduleDetailManager.GetAllByUserAndOrg(name, userId);

                        if (detail == null || detail.Count == 0)
                        {
                            throw new Exception("Not Authorized");
                        }
                    }
                    else if (user.OrganizationConsutants.All(x => x.OrganizationId != organization.Id))
                    {
                        throw new Exception("Not Authorized");
                    }
                    
                }
            }
            else if (organization == null)
            {
                organization = organizationManager.GetByName(name);
                if (organization == null)
                {
                    throw new Exception("Not Authorized");
                }
            }

            result = new AccessTokenDetail
            {
                VaultId = organization.DocumentLibraryVaultId
            };

            return result;
        }

        public void MigrateDocumentLibrary(int organizationId, string runBy)
        {
            var organizationManager = this.container.GetInstance<OrganizationManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var org = organizationManager.GetById(organizationId);

            if (org == null)
            {
                throw new Exception($"Cannot find org {organizationId}");
            }

            var cycle = 1;

            if (org.OrganizationAccreditationCycles != null)
            {
                cycle = org.OrganizationAccreditationCycles.Single(x=>x.IsCurrent).Number;
            }

            //cycle++;

            var orgName = org.Name;

            if (orgName.Length > 70)
            {
                orgName = orgName.Substring(0, 70);
            }

            var vaultName = $"{orgName} - {cycle}";

            var result = trueVaultManager.CreateOrganization(vaultName, string.Empty);

            var userOrgs = org.Users.Select(x => new UserOrganizationItem
            {
                Organization = new OrganizationItem
                {
                    OrganizationName = orgName,
                    DocumentLibraryGroupId = result.GroupId
                }
            }).ToList();

            var groups = trueVaultManager.GetAllGroups();

            foreach (var user in org.Users)
            {
                trueVaultManager.AddUserToGroups(userOrgs, user.User.DocumentLibraryUserId, groups);
            }

            var factUsers = userManager.GetFactStaff();

            foreach (var u in factUsers)
            {
                trueVaultManager.AddUserToGroups(userOrgs, u.DocumentLibraryUserId, groups);
            }

            organizationManager.CreateDocumentLibrary(organizationId, cycle, result.VaultId, result.GroupId, runBy);
        }
    }
}

