namespace FactWeb.Model.InterfaceItems
{
    public class OrganizationFacilityItems
    {
        public int OrganizationFacilityId { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public int FacilityId { get; set; }

        public  string FacilityName{ get; set; }

        public string Relation { get; set; }
        public string InspectionDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public string MasterServiceTypeName { get; set; }
        public string ServiceTypeName { get; set; }

    }
}
