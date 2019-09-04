using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationStatusHistory : BaseModel
    {
        [Key, Column("ApplicationStatusHistoryId")]
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

        public int ApplicationStatusIdOld { get; set; }
        public int ApplicationStatusIdNew { get; set; }

        [ForeignKey("ApplicationStatusIdOld")]
        public virtual ApplicationStatus ApplicationStatusOld { get; set; }

        [ForeignKey("ApplicationStatusIdNew")]
        public virtual ApplicationStatus ApplicationStatusNew { get; set; }
    }
}
