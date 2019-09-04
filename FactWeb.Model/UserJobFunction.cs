using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class UserJobFunction : BaseModel
    {
        [Key, Column("UserJobFunctionId")]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JobFunctionId { get; set; }

        public virtual User User { get; set; }
        public virtual JobFunction JobFunction { get; set; }
    }
}
