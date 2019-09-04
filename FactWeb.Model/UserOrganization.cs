using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class UserOrganization : BaseModel
    {
        [Key, Column("UserOrganizationId")]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int OrganizationId { get; set; }
        public Guid JobFunctionId { get; set; }
        [Column("UserOrganizationShowOnAccReport")]
        public bool ShowOnAccReport { get; set; }

        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual JobFunction JobFunction { get; set; }
    }
}
