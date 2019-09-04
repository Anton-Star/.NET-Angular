using System;

namespace FactWeb.Model
{
    public class SiteProcessingTotalTransplantCellType : BaseModel
    {
        public Guid Id { get; set; }
        public Guid SiteProcessingTotalId { get; set; }
        public Guid TransplantCellTypeId { get; set; }

        public virtual SiteProcessingTotal SiteProcessingTotal { get; set; }
        public virtual TransplantCellType TransplantCellType { get; set; }
    }
}
