using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class JobFunction : BaseModel
    {
        [Key, Column("JobFunctionId")]
        public Guid Id { get; set; }
        [Column("JobFunctionName")]
        public string Name { get; set; }
        [Column("JobFunctionOrder")]
        public int Order { get; set; }
        [Column("JobFunctionIsActive")]
        public bool IsActive { get; set; }
        [Column("JobFunctionIncludeInReporting")]
        public bool? IncludeInReporting { get; set; }
        [Column("JobFunctionReportingOrder")]
        public int? ReportingOrder { get; set; }

        public virtual ICollection<UserJobFunction> UserJobFunctions { get; set; }
    }
}
