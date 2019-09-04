using System;

namespace FactWeb.Model.InterfaceItems
{
    public class ProcessingTypeItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public bool? IsSelected { get; set; }
    }
}