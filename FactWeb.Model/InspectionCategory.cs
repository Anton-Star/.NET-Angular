using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class InspectionCategory : BaseModel
    {
        [Key, Column("InspectionCategoryId")]
        public int Id { get; set; }
        [Column("InspectionCategoryName")]        
        public string Name { get; set; }
        [Column("InspectionCategoryReportingOrder")]
        public int? ReportingOrder { get; set; }
    }
}
