using System;
using System.ComponentModel.DataAnnotations;

namespace FactWeb.Model
{
    public class BaseModel
    {
        [Required, MaxLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
