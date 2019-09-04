using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace FactWeb.Repository
{
    public class ApplicationResponseCommentDocumentRepository : BaseRepository<ApplicationResponseCommentDocument>, IApplicationResponseCommentDocumentRepository
    {
        public ApplicationResponseCommentDocumentRepository(FactWebContext context) : base(context)
        {
        }

        public List<CommentDocument> GetSectionDocuments(Guid applicationUniqueId, Guid sectionId)
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[2];
            paramList[0] = applicationUniqueId;
            paramList[1] = sectionId;

            var rows = objectContext.ExecuteStoreQuery<CommentDocument>(
                "EXEC usp_getApplicationSectionCommentDocuments @ApplicationUniqueId={0}, @ApplicationSectionId={1}", paramList).ToList();

            return rows;
        }

        public List<ApplicationResponseCommentDocument> GetAllForComment(int commentId)
        {
            return base.FetchMany(x => x.ApplicationResponseCommentId == commentId);
        }
    }
}
