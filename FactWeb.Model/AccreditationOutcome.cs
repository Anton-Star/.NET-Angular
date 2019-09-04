using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AccreditationOutcome : BaseModel
    {
        [Key, Column("AccreditationOutcomeId")]
        public int Id { get; set; }
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

        public int OutcomeStatusId { get; set; }

        [ForeignKey("OutcomeStatusId")]
        public virtual OutcomeStatus OutcomeStatus { get; set; }

        public int ReportReviewStatusId { get; set; }

        [ForeignKey("ReportReviewStatusId")]
        public virtual ReportReviewStatus ReportReviewStatus { get; set; }

        [Column("AccreditationOutcomeCommitteeDate")]
        public DateTime? CommitteeDate { get; set; }

        [Column("AccreditationOutcomeSendEmail")]
        public bool? SendEmail { get; set; }
    }
}