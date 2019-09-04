using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ComplianceApplicationApprovalStatus : BaseModel
    {
        [Key, Column("ComplianceApplicationApprovalStatusId")]
        public Guid Id { get; set; }
        [Column("ComplianceApplicationApprovalStatusName")]
        public string Name { get; set; }

        public virtual ICollection<ComplianceApplication> ComplianceApplications { get; set; }
    }
}
