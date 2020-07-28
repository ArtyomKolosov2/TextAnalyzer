using System.Collections.Generic;
using System.Drawing;

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
            Color.Gold,
            Color.Orange,
            Color.Aquamarine,
            Color.Aqua
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

    public class SortByStartIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel o1, EntryModel o2)
        {
            return o1.StartIndex.CompareTo(o2.StartIndex);
        }
    }
    public class EntryModel
    {
        public static int Offset { get; set; }
        private int startIndex;
        private int endIndex;
        public Color TextColor { get; set; }

        public int StartIndex
        {
            get { return startIndex + Offset; }
            set { startIndex = value; }
        }
        public int EndIndex
        {
            get { return endIndex + Offset; }
            set { endIndex = value; }
        }
        public int Length
        {
            get { return EndIndex - StartIndex; }
        }
    }

    public class EntryModelNum : EntryModel
    {
        public long Num { get; set; }
    }
}
