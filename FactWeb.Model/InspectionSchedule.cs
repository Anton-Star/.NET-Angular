using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class InspectionSchedule : BaseModel
    {
        [Key, Column("InspectionScheduleId")]
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public int OrganizationId { get; set; }

        [Column("InspectionScheduleInspectionDate")]
        public DateTime InspectionDate { get; set; }

        [Column("InspectionScheduleIsCompleted")]
        public bool IsCompleted { get; set; }
        [Column("InspectionScheduleCompletionDate")]
        public DateTime CompletionDate { get; set; }

        [Column("InspectionScheduleStartDate")]
        public DateTime StartDate { get; set; }
        [Column("InspectionScheduleEndDate")]
        public DateTime EndDate { get; set; }

        [Column("InspectionScheduleIsActive")]
        public bool IsActive { get; set; }

        [Column("InspectionScheduleIsArchive")]
        public bool IsArchive { get; set; }

        public string SiteDescription { get; set; }
        public string CommendablePractices { get; set; }
        public string OverallImpressions { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Applications { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organizations { get; set; }

        public virtual ICollection<InspectionScheduleDetail> InspectionScheduleDetails { get; set; }

    }
}
