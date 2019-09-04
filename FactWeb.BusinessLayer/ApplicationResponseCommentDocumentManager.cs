using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class ApplicationResponseCommentDocumentManager : BaseManager<ApplicationResponseCommentDocumentManager, IApplicationResponseCommentDocumentRepository, ApplicationResponseCommentDocument>
    {
        public ApplicationResponseCommentDocumentManager(IApplicationResponseCommentDocumentRepository repository) : base(repository)
        {
        }

        public List<CommentDocument> GetSectionDocuments(Guid applicationUniqueId, Guid sectionId)
        {
            return base.Repository.GetSectionDocuments(applicationUniqueId, sectionId);
        }

        public void RemoveForComment(int commentId)
        {
            var rows = base.Repository.GetAllForComment(commentId);

            foreach (var row in rows)
            {
                base.Repository.BatchRemove(row);
            }

            base.Repository.SaveChanges();
        }
    }
}
