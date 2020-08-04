using System;
using System.Collections.Generic;
using System.Drawing;
using TextAnalyzer.Modules.Models;

namespace TextAnalyzer.Modules
{
    public enum EntryCodes
    {
        LongestWord,
        LargestNumber,
        OnlyConsonat,
        OnlyVowel,
    }

    public class ColorInfo : IComparable<ColorInfo>
    {
        public string Name { get; set; }
        public string Mean { get; set; }

        public int CompareTo(ColorInfo o1)
        {
            return Mean.CompareTo(o1.Mean);
        }
        
    }

    public static class GetColor
    {
        private static CodeMeanings CodeMeanings { get; } = new CodeMeanings();

        public static Color[] textColors { get; } = new Color[]
        {
            Color.Orange,
            Color.CornflowerBlue,
            Color.GreenYellow,
            Color.Aqua,
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

        public static string GetCodeMeaning(EntryCodes code)
        {
            return CodeMeanings.codeMeaning[code];
        }

        public static EntryCodes GetCodeByColor(Color color)
        {
            EntryCodes code = default;
            for (int i = 0; i < textColors.Length; i++)
            {
                if (color.Equals(textColors[i]))
                {
                    code = (EntryCodes)i;
                    if (!Enum.IsDefined(code.GetType(), code))
                    {
                        throw new Exception("This Color is not defined in EntryCodes");
                    }
                    break;
                }
            }
            return code;
        }
    }

    public class CompareByStartIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel o1, EntryModel o2)
        {
            return o1.StartIndex.CompareTo(o2.StartIndex);
        }
    }

    public class CompareByEndIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel o1, EntryModel o2)
        {
            return o1.EndIndex.CompareTo(o2.EndIndex);
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
