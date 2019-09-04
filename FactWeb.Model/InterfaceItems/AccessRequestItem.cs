namespace FactWeb.Model.InterfaceItems
{
    public class AccessRequestItem
    {
        public string Guid { get; set; }
        public string ServiceType { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string DirectorName { get; set; }
        public string DirectorEmailAddress { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmailAddress { get; set; }
        public string PrimaryContactPhoneNumber { get; set; }
        public string MasterServiceTypeOtherComment { get; set; }
        public string OtherComments { get; set; }
    }
}
