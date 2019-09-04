using System;

namespace FactWeb.Model.InterfaceItems
{
    public class Personnel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string JobFunction { get; set; }
        public bool ShowOnAccReport { get; set; }
        public string OverrideJobFunction { get; set; }
    }
}