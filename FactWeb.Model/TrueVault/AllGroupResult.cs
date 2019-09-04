using System.Collections.Generic;

namespace FactWeb.Model.TrueVault
{
    public class AllGroupResult
    {
        public string Result { get; set; }
        public string Transaction_id { get; set; }
        public List<GroupResult> Groups { get; set; }

    }
}
