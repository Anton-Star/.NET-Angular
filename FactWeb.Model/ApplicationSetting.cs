using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactWeb.Model
{
    public class ApplicationSetting : BaseModel
    {
        [Key, Column("ApplicationSettingId")]
        public int Id { get; set; }
        [Column("ApplicationSettingName"), MaxLength(500), Required]
        public string Name { get; set; }
        [Column("ApplicationSettingValue"), MaxLength(500)]
        public string Value { get; set; }

        [Column("ApplicationSettingSystemName")]
        public string SystemName { get; set; }
    }
}
