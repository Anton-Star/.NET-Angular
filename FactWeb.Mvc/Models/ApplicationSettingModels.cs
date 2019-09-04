using FactWeb.Model;
using System.Collections.Generic;

namespace FactWeb.Mvc.Models
{
    public class ApplicationSettingUpdateModel
    {
        public string ApplicationSettingName { get; set; }
        public string ApplicationSettingValue { get; set; }
    }

    public class ApplicationSettingSaveModel
    {
        public List<ApplicationSetting> Settings { get; set; }
    }
}