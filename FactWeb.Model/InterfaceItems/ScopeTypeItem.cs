namespace FactWeb.Model.InterfaceItems
{
    public class ScopeTypeItem
    {
        public int ScopeTypeId { get; set; }
        public string Name { get; set; }
        public string ImportName { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public bool? IsSelected { get; set; }

    }
}
