using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ReportReviewStatus : BaseModel
    {
        [Key, Column("ReportReviewStatusId")]
        public int Id { get; set; }
        [Column("ReportReviewStatusName")]
        public string Name { get; set; }
    }
}

