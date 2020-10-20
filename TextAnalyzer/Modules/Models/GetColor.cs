using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextAnalyzer.Modules.Models
{
    public class ColorInfo : IComparable<ColorInfo>
    {
        public string Name { get; set; }
        public string Mean { get; set; }

        public int CompareTo(ColorInfo other)
        {
            return Name.CompareTo(other.Name);
        }
    }

    public static class GetColor
    {
        public static List<Color> TextColors { get; } = new List<Color>
        {
            Color.Orange,
            Color.GreenYellow,
            Color.Aqua,
            Color.DarkKhaki,
            Color.CornflowerBlue,
            Color.Yellow
        };

        public static Color GetColorByCode(EntryCodes code)
        {
            int integerCode = (int)code;
            Color color;
            if (integerCode < TextColors.Count)
            {
                color = TextColors[integerCode];
            }
            else
            {
                throw new InvalidOperationException("This Color is not defined!");
            }
            return color;
        }
    }
}
