using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationStatus : BaseModel
    {
        [Key, Column("ApplicationStatusId")]
        public int Id { get; set; }
        [Column("ApplicationStatusName")]
        public string Name { get; set; }
        [Column("ApplicationStatusNameForApplicant")]
        public string NameForApplicant { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
