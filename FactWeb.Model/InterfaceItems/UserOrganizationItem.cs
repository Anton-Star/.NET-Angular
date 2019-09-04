namespace FactWeb.Model.InterfaceItems
{
    public class UserOrganizationItem 
    {
        public OrganizationItem Organization { get; set; }
        public JobFunctionItem JobFunction { get; set; }
        public bool? ShowOnAccReport { get; set; }
    }
}
