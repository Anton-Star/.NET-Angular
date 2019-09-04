namespace FactWeb.Model.InterfaceItems
{
    public class UserMembershipItem
    {
        public MembershipItem Membership { get; set; }
        public string MembershipNumber { get; set; }
        public bool? IsSelected { get; set; }
    }
}
