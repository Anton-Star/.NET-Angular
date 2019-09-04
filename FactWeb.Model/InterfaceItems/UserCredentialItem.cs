using System;
using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class UserCredentialItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int CredentialId { get; set; }
    }
}

