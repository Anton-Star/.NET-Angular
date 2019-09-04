using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationSectionRepository : BaseRepository<ApplicationSection>, IApplicationSectionRepository
    {
        public ApplicationSectionRepository(FactWebContext context) : base(context)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;
        }

        public override ApplicationSection GetById(int id)
        {
            throw new NotImplementedException("Use GetById Guid");
        }

        public ApplicationSection GetById(Guid id)
        {
            return base.Dbset.Find(id);
        }

        public List<ApplicationSection> GetByParent(Guid parentId)
        {
            return
                base.FetchMany(
                    x => x.ParentApplicationSectionId == parentId && x.IsActive && x.ApplicationVersion.IsActive);
        }

        public Task<List<ApplicationSection>> GetByParentAsync(Guid parentId)
        {
            return
                base.FetchManyAsync(
                    x => x.ParentApplicationSectionId == parentId && x.IsActive);
        }

        public List<ApplicationSection> GetRootLevel()
        {
            return
                base.FetchMany(x => x.ParentApplicationSectionId == null && x.IsActive && x.ApplicationVersion.IsActive);
        }

        public Task<List<ApplicationSection>> GetRootLevelAsync()
        {
            return
                base.FetchManyAsync(
                    x => x.ParentApplicationSectionId == null && x.IsActive && x.ApplicationVersion.IsActive);
        }

        public List<ApplicationSection> GetAllForApplicationType(int applicationTypeId)
        {
            return
                base.FetchMany(
                    x =>
                        x.ApplicationTypeId == applicationTypeId && x.ParentApplicationSectionId == null && x.IsActive &&
                        x.ApplicationVersion.IsActive);
        }

        public Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(int applicationTypeId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.ApplicationTypeId == applicationTypeId && x.ParentApplicationSectionId == null && x.IsActive &&
                        x.ApplicationVersion.IsActive);
        }

        public List<ApplicationSection> GetAllForApplicationType(string applicationTypeName)
        {
            return
                base.Context.ApplicationSections.Where(
                    x =>
                        x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null &&
                        x.IsActive && x.ApplicationVersion.IsActive)
                    .Include(o => o.Children)
                    .Include(o => o.Questions)
                    .Include(o => o.Questions.Select(x => x.ScopeTypes))
                    .Include(o => o.Questions.Select(x => x.ScopeTypes.Select(y=>y.ScopeType)))
                    .Include(o => o.Questions.Select(x => x.Answers))
                    .Include(o => o.Questions.Select(x => x.HiddenBy))
                    .Include(o => o.Questions.Select(x => x.HiddenBy.Select(y => y.ApplicationSectionQuestionAnswer)))
                    .ToList();
            //return base.FetchMany(x => x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null && x.IsActive && x.ApplicationVersion.IsActive);
        }
        public ApplicationSection GetById(Guid reqGuid, int appId)
        {
            return base.Context.ApplicationSections.Where(r => r.Id == reqGuid)
                    .Include(o => o.Children)
                    .Include(o => o.Questions)
                    .Include(o => o.Questions.Select(x => x.ScopeTypes))
                    .Include(o => o.Questions.Select(x => x.ScopeTypes.Select(y => y.ScopeType)))
                    .Include(o => o.Questions.Select(x => x.Answers))
                    .Include(o => o.Questions.Select(x => x.ApplicationResponses.Select(z => z.Document)))
                    .SingleOrDefault();
        }

        public Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(string applicationTypeName)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null &&
                        x.IsActive && x.ApplicationVersion.IsActive);
        }

        public IEnumerable<ApplicationSection> GetForApplicationType(string applicationTypeName)
        {
            return
                base.Context.ApplicationSections.Where(
                    x =>
                        x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null &&
                        x.IsActive && x.ApplicationVersion.IsActive);
            //return base.FetchMany(x => x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null && x.IsActive);
        }

        public Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(string applicationTypeName, string version)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null &&
                        x.IsActive && x.ApplicationVersion.VersionNumber == version);
        }

        public List<ApplicationSection> GetAllForApplicationType(string applicationTypeName, string version)
        {
            return
                base.Context.ApplicationSections.Where(
                    x =>
                        x.ApplicationType.Name == applicationTypeName && x.ParentApplicationSectionId == null &&
                        x.IsActive && x.ApplicationVersion.VersionNumber == version)
                    .Include(o => o.Children)
                    .Include(o => o.Questions)
                    .Include(o => o.Questions.Select(x => x.ScopeTypes))
                    .Include(o => o.Questions.Select(x => x.Answers))
                    .ToList();
        }

        public List<ApplicationSection> GetAllWithDocument(Guid appId)
        {
            //return
            //    base.Context.ApplicationSections.Where(
            //            x => x.IsActive &&
            //            x.Questions.Any(y => y.IsActive && y.ApplicationResponses.Any(z => z.Application.Organization.Name == orgName && z.DocumentId != null))
            //        )
            //        .Include(o => o.Children)
            //        .Include(o => o.Questions)
            //        .Include(o => o.Questions.Select(x => x.ScopeTypes))
            //        .Include(o => o.Questions.Select(x => x.Answers))
            //        .Include(o => o.Questions.Select(x => x.ApplicationResponses))
            //        .Include(o => o.Questions.Select(x => x.ApplicationResponses.Select(z => z.Document)))
            //        .ToList();

            var questions = base.Context.ApplicationSectionQuestions
                .Where(
                    q =>
                        q.IsActive &&
                        (q.ApplicationResponses.Any(z => z.Application.UniqueId == appId && z.DocumentId != null) ||
                         q.ApplicationResponseComments.Any(
                             z =>
                                 z.Application.UniqueId == appId &&
                                 (z.DocumentId != null || z.ApplicationResponseCommentDocuments.Any())))
                )
                .ToList();

            var applicationResponses = base.Context.ApplicationResponses
                .Include(x => x.Application)
                .Include(x => x.Application.Site)
                .Where(
                    r => r.Application.UniqueId == appId && r.DocumentId != null
                )
                .Include(d => d.Document)
                .Include(x=>x.Document.AssociationTypes.Select(y=>y.AssociationType))
                .ToList();

            var sections = this.Context.ApplicationSections
                .Where(
                    s => s.IsActive &&
                         s.Questions.Any(y => y.IsActive &&
                                              (y.ApplicationResponses.Any(
                                                   z => z.Application.UniqueId == appId && z.DocumentId != null) ||
                                               y.ApplicationResponseComments.Any(
                                                   z =>
                                                       z.Application.UniqueId == appId &&
                                                       (z.DocumentId != null ||
                                                        z.ApplicationResponseCommentDocuments.Any()))))
                )
                .ToList();

            var comments = base.Context.ApplicationResponseComments
                .Include(x => x.Document)
                .Include(x => x.Document.AssociationTypes.Select(y => y.AssociationType))
                .Include(x => x.ApplicationResponseCommentDocuments)
                .Include(x => x.ApplicationResponseCommentDocuments.Select(y => y.Document))
                .Where(
                    x =>
                        x.Application.UniqueId == appId &&
                        (x.Document != null || x.ApplicationResponseCommentDocuments.Any()))
                .ToList();

            foreach (var question in questions)
            {
                question.ApplicationResponses =
                    applicationResponses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();

                question.ApplicationResponseComments = comments.Where(x => x.QuestionId == question.Id).ToList();
            }

            foreach (var section in sections)
            {
                section.Questions = questions.Where(x => x.ApplicationSectionId == section.Id).ToList();
            }

            return sections;
        }

        public Task<List<ApplicationSection>> GetAllWithDocumentAsync(int organizationId)
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.IsActive &&
                        x.Questions.Any(y => y.IsActive && y.ApplicationResponses.Any(z => z.Application.OrganizationId == organizationId && z.DocumentId != null)));
        }

        public List<ApplicationSection> GetRootItems(int applicationTypeId)
        {

            return this.Context.ApplicationSections
                .Where(x => x.ApplicationTypeId == applicationTypeId && x.ParentApplicationSectionId == null && x.IsActive == true)
                          .Include(b => b.Children)
                          .Include(b=>b.Questions)
                          .Include(x=>x.Questions.Select(y=>y.ScopeTypes))
                          .Include(x => x.Questions.Select(y => y.ScopeTypes.Select(z=>z.ScopeType)))
                          .Include(x=>x.Questions.Select(y=>y.Answers))
                          .Include(x=>x.Questions.Select(y=>y.Answers.Select(z=>z.Displays)))
                          .Include(x=>x.Questions.Select(y=>y.HiddenBy))
                          .Include(x => x.ApplicationSectionScopeTypes)
                          .Include(x => x.ApplicationSectionScopeTypes.Select(y => y.ScopeType))
                          .Include(x=>x.Questions.Select(y=>y.ApplicationResponses))
                          .Include(x=>x.Questions.Select(y=>y.ApplicationResponses.Select(z=>z.ApplicationResponseStatus)))
                          .Include(x=>x.Questions.Select(y=>y.ApplicationResponseComments))
                          .ToList();
        }

        public Task<List<ApplicationSection>> GetRootItemsAsync(int applicationTypeId)
        {
            return
                base.FetchManyAsync(
                    x => x.ApplicationTypeId == applicationTypeId && x.ParentApplicationSectionId == null && x.IsActive == true);
        }

        private List<ApplicationSection> ProcessSection(Guid parentSectionId, List<ApplicationSection> allSections, List<ApplicationSectionQuestion> questions)
        {
            var result = allSections.Where(x => x.ParentApplicationSectionId == parentSectionId).ToList();

            foreach (var section in result)
            {
                section.Children = this.ProcessSection(section.Id, allSections, questions);
                section.Questions = questions.Where(x => x.ApplicationSectionId == section.Id).ToList();
            }

            return result;
        }



        public List<ApplicationSection> GetFlatForApplicationType(int applicationTypeId, Guid? applicationVersionId)
        {
            var questions = base.Context.ApplicationSectionQuestions
                .Where(
                    x =>
                        x.ApplicationSection.ApplicationTypeId == applicationTypeId && x.IsActive &&
                        x.ApplicationSection.IsActive &&
                        (x.ApplicationSection.ApplicationVersionId == applicationVersionId ||
                         applicationVersionId == null))
                .Include(x => x.ScopeTypes)
                .Include(x => x.ScopeTypes.Select(y => y.ScopeType))
                .Include(x => x.Answers)
                .Include(x => x.QuestionType)
                .Include(x => x.ApplicationResponseComments)
                .Include(x => x.HiddenBy)
                .ToList();

            var applicationResponses = base.Context.ApplicationResponses
                .Where(
                    x =>
                        x.ApplicationSectionQuestion.ApplicationSection.ApplicationTypeId == applicationTypeId &&
                        x.ApplicationSectionQuestion.IsActive &&
                        x.ApplicationSectionQuestion.ApplicationSection.IsActive &&
                        (x.ApplicationSectionQuestion.ApplicationSection.ApplicationVersionId == applicationVersionId ||
                         applicationVersionId == null))
                .Include(x => x.ApplicationResponseStatus)
                .Include(x => x.Document)
                .ToList();

            var sections = this.Context.ApplicationSections
                .Where(
                    x =>
                        x.ApplicationTypeId == applicationTypeId && x.IsActive &&
                        (x.ApplicationVersionId == applicationVersionId || applicationVersionId == null))
                
                .Include(x => x.ApplicationSectionScopeTypes)
                .Include(x => x.ApplicationSectionScopeTypes.Select(y => y.ScopeType))
                .Include(x => x.ApplicationType)
                .ToList();

            foreach (var question in questions)
            {
                question.ApplicationResponses =
                    applicationResponses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
            }

            foreach (var section in sections)
            {
                section.Questions = questions.Where(x => x.ApplicationSectionId == section.Id).ToList();
            }
            
            return sections;
        }

        public List<ApplicationSection> GetAllActiveForApplicationType(int applicationTypeId)
        {
            return this.Context.ApplicationSections
                .Where(x => x.ApplicationTypeId == applicationTypeId && x.ParentApplicationSectionId == null && x.IsActive == true)
                          .Include(b => b.Children)
                          .Include(b => b.Questions)
                          .Include(x => x.Questions.Select(y => y.ScopeTypes))
                          .Include(x => x.Questions.Select(y => y.ScopeTypes.Select(z=>z.ScopeType)))
                          .Include(x => x.Questions.Select(y => y.Answers))
                          .Include(x => x.Questions.Select(y => y.Answers.Select(z => z.Displays)))
                          .Include(x => x.ApplicationSectionScopeTypes)
                          .Include(x => x.ApplicationSectionScopeTypes.Select(y => y.ScopeType))
                          .Include(x => x.Questions.Select(y => y.ApplicationResponses))
                          .Include(x => x.Questions.Select(y => y.ApplicationResponses.Select(z => z.ApplicationResponseStatus)))
                          .Include(x => x.Questions.Select(y => y.ApplicationResponseComments))
                          .ToList();
        }

        public List<ApplicationSection> GetAllForVersion(Guid versionId)
        {
            return this.Context.ApplicationSections
                .Where(x => x.ApplicationVersionId == versionId)
                          .Include(b => b.Children)
                          .Include(b => b.Questions)
                          .Include(x => x.Questions.Select(y => y.ScopeTypes))
                          .Include(x => x.Questions.Select(y => y.ScopeTypes.Select(z => z.ScopeType)))
                          .Include(x => x.Questions.Select(y => y.Answers))
                          .Include(x => x.Questions.Select(y => y.Answers.Select(z => z.Displays)))
                          .Include(x => x.ApplicationSectionScopeTypes)
                          .Include(x => x.ApplicationSectionScopeTypes.Select(y => y.ScopeType))
                          .Include(x => x.Questions.Select(y => y.ApplicationResponses))
                          .Include(x => x.Questions.Select(y => y.ApplicationResponses.Select(z => z.ApplicationResponseStatus)))
                          .Include(x => x.Questions.Select(y => y.ApplicationResponseComments))
                          .ToList();
        }

        public Task<List<ApplicationSection>> GetAllActiveForApplicationTypeAsync(int applicationTypeId)
        {
            return base.FetchManyAsync(x => x.ApplicationTypeId == applicationTypeId && x.IsActive);
        }

        public List<ApplicationSection> GetAllForActiveVersionsNoLazyLoad()
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationSections
                .AsNoTracking()
                .Include(x => x.ApplicationSectionScopeTypes)
                .Include(x => x.ApplicationSectionScopeTypes.Select(z => z.ScopeType))
                .Where(x=>x.ApplicationVersion.IsActive && (x.ApplicationVersion.IsDeleted == null || x.ApplicationVersion.IsDeleted == false))
                .ToList();
        }

        public List<ApplicationSection> GetAllForVersionNoLazyLoad(Guid versionId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationSections
                .AsNoTracking()
                .Include(x => x.ApplicationSectionScopeTypes)
                .Include(x => x.ApplicationSectionScopeTypes.Select(z => z.ScopeType))
                .Where(x => x.ApplicationVersionId == versionId)
                .ToList();
        }

        public List<ApplicationSection> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ApplicationSections
                .Include(x => x.ApplicationSectionScopeTypes)
                .Include(x => x.ApplicationSectionScopeTypes.Select(y => y.ScopeType))
                .Where(
                    x =>
                        x.ApplicationType.Name == applicationTypeName && (x.ApplicationVersion.IsDeleted == null || x.ApplicationVersion.IsDeleted == false))
                .ToList();
        }

        public List<ApplicationSection> GetAllWithDocumentForComp(Guid compId)
        {
            var questions = base.Context.ApplicationSectionQuestions
                .Where(
                    q =>
                        q.IsActive &&
                        (q.ApplicationResponses.Any(
                             z => z.Application.ComplianceApplicationId == compId && z.DocumentId != null) ||
                         q.ApplicationResponseComments.Any(
                             z =>
                                 z.Application.ComplianceApplicationId == compId &&
                                 (z.DocumentId != null || z.ApplicationResponseCommentDocuments.Any())))
                )
                .ToList();

            var applicationResponses = base.Context.ApplicationResponses
                .Include(x => x.Application)
                .Include(x => x.Application.Site)
                .Include(d => d.Document)
                .Include(x=>x.Document.AssociationTypes)
                .Include(x=>x.Document.AssociationTypes.Select(y=>y.AssociationType))
                .Where(
                    r => r.Application.ComplianceApplicationId == compId && r.DocumentId != null
                )
                .ToList();

            var sections = this.Context.ApplicationSections
                .Where(
                    s => s.IsActive &&
                         s.Questions.Any(y => y.IsActive &&
                                              (y.ApplicationResponses.Any(
                                                   z =>
                                                       z.Application.ComplianceApplicationId == compId &&
                                                       z.DocumentId != null) ||
                                               y.ApplicationResponseComments.Any(
                                                   z =>
                                                       z.Application.ComplianceApplicationId == compId &&
                                                       (z.DocumentId != null ||
                                                        z.ApplicationResponseCommentDocuments.Any()))))
                )
                .ToList();

            var comments = base.Context.ApplicationResponseComments
                .Include(x => x.Document)
                .Include(x => x.ApplicationResponseCommentDocuments)
                .Include(x => x.ApplicationResponseCommentDocuments.Select(y => y.Document))
                .Include(x => x.Document.AssociationTypes)
                .Include(x => x.Document.AssociationTypes.Select(y => y.AssociationType))
                .Where(
                    x =>
                        x.Application.ComplianceApplicationId == compId &&
                        (x.Document != null || x.ApplicationResponseCommentDocuments.Any()))
                .ToList();

            foreach (var question in questions)
            {
                question.ApplicationResponses =
                    applicationResponses.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();

                question.ApplicationResponseComments = comments.Where(x => x.QuestionId == question.Id).ToList();
            }

            foreach (var section in sections)
            {
                section.Questions = questions.Where(x => x.ApplicationSectionId == section.Id).ToList();
            }

            return sections;
        }

        public List<ApplicationSectionResponse> GetApplicationSectionsForApplication(Guid? complianceApplicationId, Guid? applicationUniqueId, Guid currentUserId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[3];
            paramList[0] = complianceApplicationId;
            paramList[1] = applicationUniqueId;
            paramList[2] = currentUserId;

            var rows = objectContext.ExecuteStoreQuery<ApplicationSectionResponse>(
                "EXEC usp_getApplicationSectionsForApplication @ComplianceApplicationId={0}, @ApplicationUniqueId={1}, @CurrentUserId={2}", paramList).ToList();

            return rows;
        }

        public List<ApplicationSection> GetSectionsWithRFIs(int applicationId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[1];
            paramList[0] = applicationId;

            var rows = objectContext.ExecuteStoreQuery<ApplicationSection>(
                "EXEC usp_getSectionsWithRFI @ApplicationId={0}", paramList).ToList();

            return rows;
        }
    }
}