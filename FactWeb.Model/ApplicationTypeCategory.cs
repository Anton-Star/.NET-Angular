using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationTypeCategory : BaseModel
    {
        [Key, Column("ApplicationTypeCategoryId")]
        public int Id { get; set; }
        
        public int ApplicationTypeId { get; set; }
        
        public int InspectionCategoryId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
        [ForeignKey("InspectionCategoryId")]
        public virtual InspectionCategory InspectionCategory { get; set; }
    }
}
