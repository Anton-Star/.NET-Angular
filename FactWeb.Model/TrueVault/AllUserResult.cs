using System.Collections.Generic;

namespace FactWeb.Model.TrueVault
{
    public class AllUserResult
    {
        public string Result { get; set; }
        public string Transaction_id { get; set; }
        public List<UserUserResult> Users { get; set; }
    }
}
