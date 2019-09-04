using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Application : BaseModel
    {
        [Key, Column("ApplicationId")]
        public int Id { get; set; }

        public int ApplicationStatusId { get; set; }

        public int ApplicationTypeId { get; set; }

        public int OrganizationId { get; set; }
        public Guid? ComplianceApplicationId { get; set; }
        [Column("ApplicationSubmittedDate")]
        public DateTime? SubmittedDate { get; set; }
        [Column("ApplicationDueDate")]
        public DateTime? DueDate { get; set; }

        [Column("ApplicationUniqueId")]
        public Guid UniqueId { get; set; }

        public Guid? CoordinatorId { get; set; }

        public Guid? ApplicationVersionId { get; set; }

        public int? SiteId { get; set; }
        [Column("ApplicationCycleNumber")]
        public int CycleNumber { get; set; }

        [Column("ApplicationRFIDueDate")]
        public DateTime? RFIDueDate { get; set; }

        [Column("ApplicationIsActive")]
        public bool? IsActive { get; set; }
        [Column("ApplicationTypeDetail")]
        public string TypeDetail { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }
        public virtual ComplianceApplication ComplianceApplication { get; set; }
        [ForeignKey("CoordinatorId")]
        public virtual User Coordinator { get; set; }

        [ForeignKey("ApplicationVersionId")]
        public virtual ApplicationVersion ApplicationVersion { get; set; }

        public virtual Site Site { get; set; }
        
        public virtual ICollection<InspectionSchedule> InspectionSchedules { get; set; }
        public virtual ICollection<ApplicationResponse> ApplicationResponses { get; set; }

        public virtual ICollection<ApplicationResponseTrainee> ApplicationResponsesTrainee { get; set; }
        //public virtual ICollection<SiteApplicationVersion> SiteApplicationVersions { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<ApplicationResponseComment> ApplicationResponseComments { get; set; }
        public virtual ICollection<AccreditationOutcome> AccreditationOutcomes { get; set; }
        public virtual ICollection<ApplicationQuestionNotApplicable> ApplicationQuestionNotApplicables { get; set; }
        public virtual ICollection<Inspection> Inspections { get; set; }

    }
}
