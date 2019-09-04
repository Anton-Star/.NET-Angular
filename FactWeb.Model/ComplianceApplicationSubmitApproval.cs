using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ComplianceApplicationSubmitApproval : BaseModel
    {
        [Key, Column("ComplianceApplicationSubmitApprovalId")]
        public Guid Id { get; set; }
        public Guid ComplianceApplicationId { get; set; }
        public Guid UserId { get; set; }
        public bool IsApproved { get; set; }

        public virtual ComplianceApplication ComplianceApplication { get; set; }
        public virtual User User { get; set; }
    }
}
