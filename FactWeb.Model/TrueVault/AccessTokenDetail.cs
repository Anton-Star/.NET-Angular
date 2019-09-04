using System;

namespace FactWeb.Model.TrueVault
{
    public class AccessTokenDetail
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string VaultId { get; set; }
        public string UserId { get; set; }
    }
}
