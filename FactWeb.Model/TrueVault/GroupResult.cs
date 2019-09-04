using System.Collections.Generic;

namespace FactWeb.Model.TrueVault
{
    public class GroupFullResult
    {
        public string Result { get; set; }
        public string Transaction_id { get; set; }
        public GroupResult Group { get; set; }
    }

    public class GroupResult
    {
        public List<PolicyResult> Policy { get; set; }
        public List<string> User_ids { get; set; }
        public string Group_id { get; set; }
        public string Name { get; set; }
    }

    public class PolicyResult
    {
        public string Activities { get; set; }
        public List<string> Resources { get; set; }
    }
}
