using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TextAnalyzer.Models
{
    public enum EntryCodes
    {
        LongestWord,
        LongestNumber,
        OnlyConsonat,
        OnlyVowel
    }

    public static class GetColor
    {
        private static Color [] textColors = new Color[]
        {
            Colors.Gold,
            Colors.Orange,
            Colors.Aquamarine,
            Colors.Aqua
        };
            
        public static Color GetColorByCode(EntryCodes code) 
        {
            Color color = default;
            int integerCode = (int)code;
            if (integerCode < textColors.Length)
            {
                color = textColors[integerCode];
            }
            return color;
        }
    }
    public class EntryModel
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public Color textColor { get; set; }
    }
}
