using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ComplianceApplication : BaseModel
    {
        [Key, Column("ComplianceApplicationId")]
        public Guid Id { get; set; }

        public Guid ComplianceApplicationApprovalStatusId { get; set; }
        public int OrganizationId { get; set; }
        public int ApplicationStatusId { get; set; }
        public Guid CoordinatorId { get; set; }

        [Column("ComplianceApplicationAccreditationGoal")]
        public string AccreditationGoal { get; set; }
        [Column("ComplianceApplicationInspectionScope")]
        public string InspectionScope { get; set; }
        [Column("ComplianceApplicationRejectionComments")]
        public string RejectionComments { get; set; }

        [Column("ComplianceApplicationIsActive")]
        public bool IsActive { get; set; }

        
        public int? ReportReviewStatusId { get; set; }

        [Column("ComplianceApplicationShowAccreditationReport")]
        public bool? ShowAccreditationReport { get; set; }

        //[Column("ComplianceApplicationAccreditationStatus")]
        //public string AccreditationStatus { get; set; }

        public virtual ComplianceApplicationApprovalStatus ComplianceApplicationApprovalStatus { get; set; }

        [ForeignKey("CoordinatorId")]
        public virtual User Coordinator { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }

        [ForeignKey("ReportReviewStatusId")]
        public virtual ReportReviewStatus ReportReviewStatus { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<ComplianceApplicationInspectionDetail> InspectionDetails { get; set; }
    }
}
