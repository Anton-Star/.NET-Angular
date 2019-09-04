using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class InspectionScheduleSite : BaseModel
    {
        [Key, Column("InspectionScheduleFacilityId")]
        public int Id { get; set; }

        public int SiteID { get; set; }

        public int InspectionScheduleId { get; set; }
        [Column("InspectionScheduleFacilityInspectionDate")]
        public DateTime InspectionDate { get; set; }       

        [ForeignKey("SiteID")]
        public virtual Site Site { get; set; }
        [ForeignKey("InspectionScheduleId")]
        public virtual InspectionSchedule InspectionSchedule { get; set; }

    }
}
