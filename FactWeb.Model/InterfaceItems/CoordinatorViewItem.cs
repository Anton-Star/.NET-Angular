using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class CoordinatorViewItem
    {
        public List<Personnel> Personnel { get; set; }
        public Overview Overview { get; set; }
        public Statistics Statistics { get; set; }
        public ComplianceApplicationItem ComplianceApplication { get; set; }
        public List<InspectionTeamMember> InspectionTeamMembers { get; set; }

    }
}
