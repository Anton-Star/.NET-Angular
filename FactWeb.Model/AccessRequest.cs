using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class AccessRequest : BaseModel
    {
        [Key, Column("RequestAccessId")]
        public string Guid { get; set; }
        public int? MasterServiceTypeId { get; set; }
        [Column("RequestAccessOrganizationName")]
        public string OrganizationName { get; set; }
        [Column("RequestAccessOrganizationAddress")]
        public string OrganizationAddress { get; set; }
        [Column("RequestAccessDirectorName")]
        public string DirectorName { get; set; }
        [Column("RequestAccessDirectorEmailAddress")]
        public string DirectorEmailAddress { get; set; }
        [Column("RequestAccessPrimaryContactName")]
        public string PrimaryContactName { get; set; }
        [Column("RequestAccessPrimaryContactEmailAddress")]
        public string PrimaryContactEmailAddress { get; set; }
        [Column("RequestAccessPrimaryContactPhoneNumber")]
        public string PrimaryContactPhoneNumber { get; set; }
        [Column("RequestAccessMasterServiceTypeOtherComment")]
        public string MasterServiceTypeOtherComment { get; set; }
        [Column("RequestAccessOtherComments")]
        public string OtherComments { get; set; }

        public MasterServiceType MasterServiceType { get; set; }

    }
}
