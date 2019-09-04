using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class InspectionScheduleDetail : BaseModel
    {
        [Key, Column("InspectionScheduleDetailId")]
        public int Id { get; set; }

        public int InspectionScheduleId { get; set; }

        public Guid UserId { get; set; }
        public int AccreditationRoleId { get; set; }

        public int InspectionCategoryId { get; set; }
        
        public int? SiteId { get; set; }

        [Column("InspectionScheduleDetailIsActive")]
        public bool IsActive { get; set; }

        [Column("InspectionScheduleDetailIsArchive")]
        public bool IsArchive { get; set; }

        [Column("InspectionScheduleDetailIsLead")]
        public bool IsLead { get; set; }

        [Column("InspectionScheduleDetailIsMentor")]
        public bool IsMentor { get; set; }

        [Column("InspectionScheduleDetailIsInspectionComplete")]
        public bool? IsInspectionComplete { get; set; }
        [Column("InspectionScheduleDetailInspectorCompletionDate")]
        public DateTime? InspectorCompletionDate { get; set; }

        [Column("InspectionScheduleDetailMentorFeedback")]
        public string MentorFeedback { get; set; }

        [Column("InspectionScheduleDetailLastReminderDate")]
        public DateTime? LastReminderDate { get; set; }

        [Column("InspectionScheduleDetailReviewedOutcomesData")]
        public bool? ReviewedOutcomesData { get; set; }

        [ForeignKey("InspectionScheduleId")]
        public virtual InspectionSchedule InspectionSchedule { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("AccreditationRoleId")]
        public virtual AccreditationRole AccreditationRole { get; set; }
        [ForeignKey("InspectionCategoryId")]
        public virtual InspectionCategory InspectionCategory { get; set; }
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
    }
}
