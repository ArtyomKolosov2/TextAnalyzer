using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextAnalyzer.Modules.Models
{
    public class CompareByStartIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel x, EntryModel y)
        {
            return x.StartIndex.CompareTo(y.StartIndex);
        }
    }

    public class CompareByEndIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel x, EntryModel y)
        {
            return x.EndIndex.CompareTo(y.EndIndex);
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

    public class EntryModelNum : EntryModel , IComparable<EntryModelNum>
    {
        public long Num { get; set; }

        public int CompareTo(EntryModelNum other)
        {
            return Num.CompareTo(other.Num);
        }
    }
}
