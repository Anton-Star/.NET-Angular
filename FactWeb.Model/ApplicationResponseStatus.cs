using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationResponseStatus : BaseModel
    {
        [Key, Column("ApplicationResponseStatus")]
        public int Id { get; set; }
        [Column("ApplicationResponseStatusName")]
        public string Name { get; set; }
        [Column("ApplicationResponseStatusNameForApplicant")]
        public string NameForApplicant { get; set; }
        [Column("ApplicationResponseStatusDescription")]
        public string Description { get; set; }
    }
}