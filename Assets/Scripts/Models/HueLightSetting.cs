using UnityEngine;

namespace Assets.Scripts.Models
{
    public class HueLightSetting
    {
        public int LightId { get; set; }
        public int SpectrumBand { get; set; }
        public float MinBrightness { get; set; }
        public float Reactivity { get; set; }
        public Color32 Color { get; set; }
        public bool AutoColor { get; set; }
    }
}
