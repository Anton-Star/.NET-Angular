using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationResponseCommentDocument : BaseModel
    {
        [Key, Column("ApplicationResponseCommentDocumentId")]
        public Guid Id { get; set; }

        public int ApplicationResponseCommentId { get; set; }
        public Guid DocumentId { get; set; }

        public virtual ApplicationResponseComment ApplicationResponseComment { get; set; }
        public virtual Document Document { get; set; }

    }
}
