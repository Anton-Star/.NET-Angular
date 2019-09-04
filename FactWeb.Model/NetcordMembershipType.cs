using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class NetcordMembershipType : BaseModel
    {
        [Key, Column("NetcordMembershipTypeId")]
        public Guid Id { get; set; }
        [Column("NetcordMembershipTypeName")]
        public string Name { get; set; }

        //public virtual ICollection<OrganizationNetcordMembership> OrganizationNetcordMemberships { get; set; }
    }
}
