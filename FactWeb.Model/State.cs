using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class State : BaseModel
    {
        [Key, Column("StateId")]
        public int Id { get; set; }
        [Column("StateName")]
        public string Name { get; set; }
    }
}
