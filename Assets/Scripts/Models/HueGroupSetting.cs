using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class HueGroupSetting
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public List<HueLightProfile> LightProfiles { get; set; }

        public static HueGroupSetting GetDefault(string groupId, string name)
        {
            return new HueGroupSetting
            {
                GroupId = groupId,
                GroupName = name,
                LightProfiles = new List<HueLightProfile>
                {
                    new HueLightProfile
                    {
                        Name = "EDM - 6 Lights",
                        LightSettings = new List<HueLightSetting>
                        {
                            new HueLightSetting
                            {
                                LightId = 0,
                                SpectrumBand = 0,
                                MinBrightness = 0.0f,
                                Reactivity = 0.2f,
                                Color = Color.red
                            },
                            new HueLightSetting
                            {
                                LightId = 1,
                                SpectrumBand = 0,
                                MinBrightness = 0.0f,
                                Reactivity = 0.2f,
                                Color = Color.red
                            },
                            new HueLightSetting
                            {
                                LightId = 2,
                                SpectrumBand = 12,
                                MinBrightness = 0.0f,
                                Reactivity = 0.2f,
                                Color = Color.red
                            },
                            new HueLightSetting
                            {
                                LightId = 3,
                                SpectrumBand = 12,
                                MinBrightness = 0.0f,
                                Reactivity = 0.2f,
                                Color = Color.red
                            },
                            new HueLightSetting
                            {
                                LightId = 4,
                                SpectrumBand = 18,
                                MinBrightness = 0.0f,
                                Reactivity = 0.2f,
                                Color = Color.red
                            },
                            new HueLightSetting
                            {
                                LightId = 5,
                                SpectrumBand = 24,
                                MinBrightness = 0.0f,
                                Reactivity = 0.2f,
                                Color = Color.red
                            }
                        }
                    }
                }
            };
        }
    }
}
