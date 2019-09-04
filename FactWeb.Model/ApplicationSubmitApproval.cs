using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationSubmitApproval : BaseModel
    {
        [Key, Column("ApplicationSubmitApprovalId")]
        public Guid Id { get; set; }

        public int ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public bool IsApproved { get; set; }

        public virtual Application Application { get; set; }
        public virtual User User { get; set; }
    }
}
