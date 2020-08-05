using System;
using System.Drawing;

namespace TextAnalyzer.Modules.Models
{
    public class ColorInfo : IComparable<ColorInfo>
    {
        public string Name { get; set; }
        public string Mean { get; set; }

        public int CompareTo(ColorInfo o1)
        {
            return Name.CompareTo(o1.Name);
        }
    }

    public static class GetColor
    {
        public static Color[] textColors = new Color[]
        {
            Color.Orange,
            Color.CornflowerBlue,
            Color.GreenYellow,
            Color.Aqua,
            Color.Cyan
        };

        public static Color GetColorByCode(EntryCodes code)
        {
            int integerCode = (int)code;
            Color color;
            if (integerCode < textColors.Length)
            {
                color = textColors[integerCode];
            }
            else
            {
                throw new Exception("This Color is not defined!");
            }
            return color;
        }
    }
}
