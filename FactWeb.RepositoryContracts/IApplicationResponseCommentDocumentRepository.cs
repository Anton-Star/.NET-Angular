using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationResponseCommentDocumentRepository : IRepository<ApplicationResponseCommentDocument>
    {
        List<CommentDocument> GetSectionDocuments(Guid applicationUniqueId, Guid sectionId);

        List<ApplicationResponseCommentDocument> GetAllForComment(int commentId);
    }
}
