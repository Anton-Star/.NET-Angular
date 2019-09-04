using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class Inspection : BaseModel
    {
        [Key, Column("InspectionId")]
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        [Column("InspectionCommendablePractices")]
        public string CommendablePractices { get; set; }

        [Column("InspectionOverallImpressions")]
        public string OverallImpressions { get; set; }
        [Column("InspectionIsTrainee")]
        public bool? IsTrainee { get; set; }
        [Column("InspectionTraineeSiteDescription")]
        public string TraineeSiteDescription { get; set; }
        [Column("InspectionIsReinspection")]
        public bool? IsReinspection { get; set; }

        [Column("InspectionOverridenImpressions")]
        public string OverridenImpressions { get; set; }
        [Column("InspectionOverridenPractices")]
        public string OverridenPractices { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

        //public Guid? SiteApplicationVersionId { get; set; }

        //[ForeignKey("SiteApplicationVersionId")]
        //public virtual SiteApplicationVersion SiteApplicationVersion { get; set; }

        public Guid InspectorId { get; set; }

        [ForeignKey("InspectorId")]
        public virtual User User { get; set; }
    }
}

