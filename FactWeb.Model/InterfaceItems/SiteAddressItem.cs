using System.Collections.Generic;

namespace FactWeb.Model.InterfaceItems
{
    public class SiteAddressItem
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int AddressId { get; set; }
        public bool? IsPrimaryAddress { get; set; }
        public string AddressType { get; set; }
        public virtual AddressItem Address { get; set; }
    }
}

