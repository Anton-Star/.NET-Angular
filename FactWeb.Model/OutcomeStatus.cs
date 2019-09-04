using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class OutcomeStatus : BaseModel
    {
        [Key, Column("OutcomeStatusId")]
        public int Id { get; set; }
        [Column("OutcomeStatusName")]
        public string Name { get; set; }
    }
}

