using Q42.HueApi.ColorConverters;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToUnityColor(this RGBColor color)
        {
            return new Color((float)color.R, (float) color.G, (float) color.B);
        }

        public static RGBColor ToHueColor(this Color color)
        {
            return new RGBColor(color.r, color.g, color.b);
        }
    }
}
