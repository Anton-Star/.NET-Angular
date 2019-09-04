using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class InspectionTeamMember
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public List<string> Names { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }

    }
}