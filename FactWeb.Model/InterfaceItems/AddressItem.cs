using System;

namespace FactWeb.Model.InterfaceItems
{
    public class AddressItem
    {
        public int Id { get; set; }
        public AddressTypeItem AddressType { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public StateItem State { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public CountryItem Country { get; set; }
        public string Logitude { get; set; }
        public string Latitude { get; set; }
        public int LocalId { get; set; }
    }
}

