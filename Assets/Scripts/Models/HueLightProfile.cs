using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class HueLightProfile
    {
        public string Name { get; set; }
        public List<HueLightSetting> LightSettings { get; set; }
    }
}
