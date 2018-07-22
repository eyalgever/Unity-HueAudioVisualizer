using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class HueProfile
    {
        public string HueKey { get; set; }
        public string HueStreamingKey { get; set; }
        public List<HueGroupSetting> GroupSettings { get; set; }

        public static HueProfile GetDefaultProfile()
        {
            return new HueProfile();
        }
    }
}
