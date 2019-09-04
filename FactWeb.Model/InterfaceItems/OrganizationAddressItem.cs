
namespace FactWeb.Model.InterfaceItems
{
    public class OrganizationAddressItem
    {
        public int Id { get; set; }
        public int AddressTypeId { get; set; }        
        public string Street1 { get; set; }        
        public string Street2 { get; set; }        
        public string City { get; set; }                
        public string State { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }        
        public string Country { get; set; }        
        public string Logitude { get; set; }        
        public string Latitude { get; set; }
        
    }
}
