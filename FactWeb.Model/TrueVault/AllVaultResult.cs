﻿using System.Collections.Generic;

namespace FactWeb.Model.TrueVault
{
    public class AllVaultResult
    {
        public string Result { get; set; }
        public string Transaction_id { get; set; }
        public List<Vault> Vaults { get; set; }
    }
}
